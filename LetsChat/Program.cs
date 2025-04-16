using LetsChat.Auth.Services;
using LetsChat.Exceptions.Handler;
using LetsChat.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var assembly = typeof(Program).Assembly;

        builder.Services.AddCarter();
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        builder.Services.AddScoped<IJwtService, JwtService>();

        builder.Services.AddDbContext<LetsChatDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
                       .WithOrigins("http://localhost:4200"));
        });

        builder.Services.AddSignalR();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddRateLimiter(options =>
        {
            // Define rate limiting policies here
            options.AddFixedWindowLimiter(policyName: "fixed", options =>
            {
                options.PermitLimit = 5; // Maximum number of requests allowed
                options.Window = TimeSpan.FromMinutes(1); // Time window for the limit
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 2;
            });

            // Global rate limiting policy (applied if no specific policy is set on an endpoint)
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString()!, // Limit by IP address
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromSeconds(30),
                        PermitLimit = 10,
                        QueueLimit = 1,
                        AutoReplenishment = true
                    }));

            // Optional: Customize the response when a request is rate-limited
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter = $"{retryAfter.TotalSeconds:0}";
                }
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
            };
        });

        // Configure JWT authentication
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseCors("AllowAll");

        app.UseRateLimiter();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapCarter();
        app.MapHub<ChatHub>("/chathub")
            .RequireAuthorization();

        app.UseExceptionHandler(options => { });

        app.Run();

    }
}