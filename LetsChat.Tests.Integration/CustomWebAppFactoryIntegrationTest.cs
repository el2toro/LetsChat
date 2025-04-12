using Microsoft.AspNetCore.Hosting;
using LetsChat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LetsChat.Tests.Integration;

public class CustomWebAppFactoryIntegrationTest : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Modify services specifically for testing (e.g., in-memory database)
        builder.ConfigureServices(services =>
        {
            var options = new DbContextOptionsBuilder<LetsChatDbContext>()
                .UseInMemoryDatabase("IntegrationTestDb")
                .Options;

            services.AddSingleton(options);
            services.AddDbContext<LetsChatDbContext>(opt => opt.UseInMemoryDatabase("IntegrationTestDb"));
        });
    }
}
