using LetsChat.Auth.Services;
using LetsChat.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LetsChat.Tests.Unit;

public class CustomWebAppFactoryUnitTest : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Modify services specifically for testing (e.g., in-memory database)
        builder.ConfigureServices(services =>
        {
            var options = new DbContextOptionsBuilder<LetsChatDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            services.AddSingleton(options);
            services.AddDbContext<LetsChatDbContext>(opt => opt.UseInMemoryDatabase("TestDb"));
            services.AddScoped<IJwtService, JwtService>();
        });


    }
}
