using LetsChat.Auth.Dtos;
using LetsChat.Auth.Login;
using LetsChat.Auth.Services;
using LetsChat.Exceptions;
using LetsChat.Models;
using LetsChat.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace LetsChat.Tests.Unit.Auth.Login;

public class LoginHandlerTest : IClassFixture<CustomWebAppFactoryUnitTest>
{
    private readonly Mock<IAuthenticationRepository> _authenticationRepository;
    private readonly LoginHandler _loginHandler;
    private readonly IJwtService _jwtService;
    public LoginHandlerTest(CustomWebAppFactoryUnitTest factory)
    {
        _authenticationRepository = new Mock<IAuthenticationRepository>();
        _jwtService = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IJwtService>();
        _loginHandler = new LoginHandler(_authenticationRepository.Object, _jwtService);
    }

    [Fact]
    public async Task Handle_Should_Return_LoginResult_When_User_Exists()
    {
        var loginDto = new LoginDto
        {
            UserName = "testuser",
            Password = "password"
        };

        var user = new User
        {
            Id = 1,
            Name = "Test",
            Surname = "User",
            Username = "testuser"
        };

        _authenticationRepository.Setup(repo => repo.Login(loginDto)).ReturnsAsync(user);

        var result = await _loginHandler.Handle(new LoginQuery(loginDto), CancellationToken.None);

        Assert.NotNull(result.LoginDto);
        Assert.Equal(1, result.LoginDto.UserId);
        Assert.Equal("Test User", result.LoginDto.FullName);
        Assert.NotEmpty(result.LoginDto.Token);
    }

    [Fact]
    public async Task Handles_When_User_Not_Exists_Throws_UserNotFoundException()
    {
        var loginDto = new LoginDto
        {
            UserId = 9969,
            UserName = "testuser",
            Password = "password"
        };

        User? user = null;

        _authenticationRepository.Setup(repo => repo.Login(loginDto)).ReturnsAsync(user);

        var result =  await Assert.ThrowsAsync<UserNotFoundException>(async () =>
        {
            await _loginHandler.Handle(new LoginQuery(loginDto), CancellationToken.None);
        });

        Assert.Equal("Entity \"User\" (9969) was not found.", result.Message);
    }
}
