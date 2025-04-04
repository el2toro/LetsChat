using LetsChat.Auth.Services;
using LetsChat.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddDbContext<LetsChatDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .WithOrigins("http://localhost:4200"));
});

builder.Services.AddSignalR();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();
app.MapHub<ChatHub>("/chathub")
    .RequireAuthorization();

app.Run();
