using LetsChat.Data;
using LetsChat.Exceptions;
using LetsChat.Models;
using LetsChat.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsChat.Tests.Unit.Repositories;

public class UserRepositoryTest
{
    [Fact]
    async Task CreateUser_Should_Add_User_To_Database()
    {
        var context = GetInMemoryDbContext();

        var user = new User
        {
            Email = "mail",
            Name = "Test",
            Surname = "Test 2",
            Username = "Testing",
            Password = "password"
        };

        var userRepository = new UserRepository(context, GetConfiguration());

        await userRepository.CreateUser(user, CancellationToken.None);

        var createdUser = await context.Users.FindAsync(user.Id);

        Assert.NotNull(createdUser);
        Assert.Equal(1, createdUser.Id);
        Assert.Equal(user.Username, createdUser.Username);
    }

    [Fact]
    async Task UpdateUser_Should_Update_User_In_Database()
    {
        var context = AddUsersContext();

        var user = new User
        {
            Id = 1,
            Email = "mail",
            Name = "Test",
            Surname = "Test 2",
            Username = "Testing",
            Password = "password"
        };

        var userRepository = new UserRepository(context, GetConfiguration());
        var updatedUser = await userRepository.UpdateUser(user, CancellationToken.None);

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
        var context = AddUsersContext();
        var userRepository = new UserRepository(context, GetConfiguration());

        var user = await userRepository.GetUserById(1, CancellationToken.None);

        Assert.NotNull(user);
        Assert.Equal(1, user.Id);
    }

    [Fact]
    async Task GetUserById_When_User_NotFound_Throws_UserNotFoundException()
    {
        var context = AddUsersContext();

        var userRepository = new UserRepository(context, GetConfiguration());

        var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
        userRepository.GetUserById(10, CancellationToken.None));

        Assert.NotNull(exception);
        Assert.Equal("Entity \"User\" (10) was not found.", exception.Message);
    }

    [Fact]
    async Task DeleteUser_Should_Remove_User_From_Database()
    {
        var context = AddUsersContext();

        var userRepository = new UserRepository(context, GetConfiguration());

        await userRepository.DeleteUser(1, CancellationToken.None);
        var result = context.Users.Find(1);

        Assert.Null(result);
    }

    [Fact]
    async Task DeleteUser_When_User_NotFound_Throws_UserNotFoundException()
    {
        var context = AddUsersContext();

        var userRepository = new UserRepository(context, GetConfiguration());

        var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
        userRepository.DeleteUser(5, CancellationToken.None));

        Assert.NotNull(exception);
        Assert.Equal("Entity \"User\" (5) was not found.", exception.Message);
    }

    [Fact]
    async Task GetUsers_Should_Return_Users()
    {
        var context = AddUsersContext();
        var userRepository = new UserRepository(context, GetConfiguration());

        var users = await userRepository.GetUsers(CancellationToken.None);

        Assert.NotNull(users);
        Assert.Equal(2, users.Count());
    }

    private LetsChatDbContext AddUsersContext()
    {
        var context = GetInMemoryDbContext();

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

        context.Users.AddRange(users);
        context.SaveChanges();

        return context;
    }

    private LetsChatDbContext GetInMemoryDbContext()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<LetsChatDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        var context = new LetsChatDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }

    private IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
           .AddJsonFile("appsettings.test.json")
           .Build();
    }
}
