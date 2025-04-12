using LetsChat.Data;
using LetsChat.Exceptions;
using LetsChat.Models;
using LetsChat.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LetsChat.Tests.Unit.Repositories;

public class UserRepositoryTest : IClassFixture<CustomWebAppFactoryUnitTest>
{
    private readonly LetsChatDbContext _context;
    private readonly DbContextOptions<LetsChatDbContext> _dbContextOptions;
    private readonly UserRepository _userRepository;
    public UserRepositoryTest(CustomWebAppFactoryUnitTest factory)
    {
        var scope = factory.Services.CreateScope();

        _context = scope.ServiceProvider.GetRequiredService<LetsChatDbContext>();
        _dbContextOptions = scope.ServiceProvider.GetRequiredService<DbContextOptions<LetsChatDbContext>>();
        _userRepository = new UserRepository(_context, _dbContextOptions);

        AddUsersToDb();
    }

    [Fact]
    async Task CreateUser_Should_Add_User_To_Database()
    {
        var user = new User
        {
            Email = "mail",
            Name = "Test",
            Surname = "Test 2",
            Username = "Testing",
            Password = "password"
        };

        await _userRepository.CreateUser(user, CancellationToken.None);

        var createdUser = await _context.Users.FindAsync(user.Id);

        Assert.NotNull(createdUser);
        Assert.Equal(3, createdUser.Id);
        Assert.Equal(user.Username, createdUser.Username);
    }

    [Fact]
    async Task UpdateUser_Should_Update_User_In_Database()
    {
        var user = new User
        {
            Id = 1,
            Email = "mail",
            Name = "Test",
            Surname = "Test 2",
            Username = "Testing",
            Password = "password"
        };

        var updatedUser = await _userRepository.UpdateUser(user, CancellationToken.None);

        Assert.NotNull(updatedUser);
        Assert.Equal(user.Id, updatedUser.Id);
        Assert.Equal(user.Email, updatedUser.Email);
        Assert.Equal(user.Name, updatedUser.Name);
        Assert.Equal(user.Surname, updatedUser.Surname);
        Assert.Equal(user.Username, updatedUser.Username);
        Assert.Equal(user.Password, updatedUser.Password);
    }

    [Fact]
    async Task GetUserById_Should_Return_User_From_Database()
    {
        var user = await _userRepository.GetUserById(1, CancellationToken.None);

        Assert.NotNull(user);
        Assert.Equal(1, user.Id);
    }

    [Fact]
    async Task GetUserById_When_User_NotFound_Throws_UserNotFoundException()
    {
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
        _userRepository.GetUserById(10, CancellationToken.None));

        Assert.NotNull(exception);
        Assert.Equal("Entity \"User\" (10) was not found.", exception.Message);
    }

    [Fact]
    async Task DeleteUser_Should_Remove_User_From_Database()
    {
        await _userRepository.DeleteUser(1, CancellationToken.None);
        var result = _context.Users.Find(1);

        Assert.Null(result);
    }

    [Fact]
    async Task DeleteUser_When_User_NotFound_Throws_UserNotFoundException()
    {
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
        _userRepository.DeleteUser(555, CancellationToken.None));

        Assert.NotNull(exception);
        Assert.Equal("Entity \"User\" (555) was not found.", exception.Message);
    }

    [Fact]
    async Task GetUsers_Should_Return_Users()
    {
        var users = await _userRepository.GetUsers(CancellationToken.None);

        Assert.NotNull(users);
        Assert.True(users.Count() > 1);
    }

    private void AddUsersToDb()
    {
        var users = new List<User>()
        {
            new User
            {
                Email = "gerry@gmail.com",
                Name = "Gerry",
                Surname = "Ateko",
                Username = "gerriko",
                Password = "123456"
            },
             new User
            {
                Email = "delgado@gmail.com",
                Name = "Anny",
                Surname = "Delgado",
                Username = "anilor",
                Password = "321654"
            },
        };

        _context.Users.AddRange(users);
        _context.SaveChanges();
    }
}
