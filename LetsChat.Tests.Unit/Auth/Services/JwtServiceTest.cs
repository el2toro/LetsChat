using LetsChat.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LetsChat.Tests.Unit.Auth.Services;

public class JwtServiceTest : IClassFixture<CustomWebAppFactoryUnitTest>
{
    private readonly IJwtService _jwtService;
    public JwtServiceTest(CustomWebAppFactoryUnitTest factory)
    {
        _jwtService = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IJwtService>();
    }

    [Fact]
    void GenerateToken_Should_Create_Jwt_Token()
    {
        var token = _jwtService.GenerateToken(1, "test");

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

}
