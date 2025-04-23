using LetsChat.Auth.Dtos;
using LetsChat.Data;
using LetsChat.Exceptions;
using LetsChat.Intefaces;
using LetsChat.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LetsChat.Tests.Unit.Repositories;

public class AuthenticationRepositoryTest : IClassFixture<CustomWebAppFactoryUnitTest>
{
    private readonly LetsChatDbContext _dbContext;
    private readonly IAuthenticationRepository _authenticationRepository;
    public AuthenticationRepositoryTest(CustomWebAppFactoryUnitTest factory)
    {
        var scope = factory.Services.CreateScope();

        _dbContext = scope.ServiceProvider.GetRequiredService<LetsChatDbContext>();
        _authenticationRepository = scope.ServiceProvider.GetRequiredService<IAuthenticationRepository>();

        AddUsersToDb();
    }

    [Fact]
    async Task Login_Should_Return_User_When_Valid_Credentials()
    {
        var loginDto = new LoginDto
        {
            UserName = "testuser1",
            Password = "password1234"
        };

        var user = await _authenticationRepository.Login(loginDto);

        Assert.NotNull(user);
        Assert.Equal("testuser1", user.Username);
        Assert.Equal("password1234", user.Password);
    }

    [Fact]
    async Task Login_Should_Throw_NotFoundException_When_User_Not_Found()
    {
        var loginDto = new LoginDto
        {
            UserName = "nonexistentuser",
            Password = "wrongpassword"
        };

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _authenticationRepository.Login(loginDto));

        Assert.Equal("Entity \"User\" (nonexistentuser) was not found.", exception.Message);
    }

    private void AddUsersToDb()
    {
        _dbContext.Users.Add(new User
        {
            Username = "testuser1",
            Password = "password1234",
            Email = "mymail@gmail.com",
            Name = "Test",
            Surename = "Test 2"
        });

        _dbContext.SaveChanges();
    }
}
