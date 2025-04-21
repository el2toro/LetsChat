using LetsChat.Data;
using LetsChat.Dtos;
using LetsChat.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LetsChat.Tests.Integration.Users;

public class UsersTest : IClassFixture<CustomWebAppFactoryIntegrationTest>
{
    private readonly HttpClient _client;
    private readonly LetsChatDbContext _context;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    public UsersTest(CustomWebAppFactoryIntegrationTest factory)
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _client = factory.CreateClient();
        _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<LetsChatDbContext>();

        SeedDatabase();
    }

    [Fact]
    async Task GetUserById_Should_Return_UserDto()
    {
        var newUser = new User
        {
            Email = "gerry@gmail.com",
            Name = "Gerrypp",
            Surename = "Atekop",
            Username = "testing99",
            Password = "555555"
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        var response = await _client.GetAsync($"user/{newUser.Id}");
        var json = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserDto>(json, _jsonSerializerOptions);

        Assert.NotNull(user);
        Assert.Equal(newUser.Id, user.Id);
        Assert.Equal("testing99", user.Username);
    }

    [Fact]
    async Task GetUserById_When_User_Doesnt_Exists_Return_NotFound()
    {
        var response = await _client.GetAsync("user/556");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    async Task CreateUser_Should_Return_StatusCode_201()
    {
        var request = new UserDto
        {
            Email = "user@gmail.com",
            Name = "New user",
            Surename = "useruser",
            Username = "User",
            Password = "Password1",
        };

        var content = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PostAsync("user/", content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    //TODO: Validation tests
    [Fact]
    async Task CreateUser_When_Email_Invalid_Return_BadRequest()
    {
        //var request = new UserDto
        //{
        //    Email = "usergmail.com",
        //    Name = "New user",
        //    Surename = "useruser",
        //    Username = "User",
        //    Password = "Password1",
        //};

        //var content = new StringContent(JsonSerializer.Serialize(request),
        //    Encoding.UTF8,
        //    "application/json");

        //var response = await _client.PostAsync("user/", content);
        //var json = await response.Content.ReadAsStringAsync();
        //var errorResponse = JsonSerializer.Deserialize<ValidationException>(json, _jsonSerializerOptions);

        //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Fact]
    async Task UpdateUser_Should_Return_StatusCode_200()
    {
        var request = new UserDto
        {
            Id = 2,
            Email = "user@gmail.com",
            Name = "Updated user",
            Surename = "User",
            Username = "user",
            Password = "1111"
        };

        var content = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PutAsync("user/", content);
        var json = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserDto>(json, _jsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(user);
        Assert.Equal(2, user.Id);
        Assert.Equal("user", user.Username);
        Assert.Equal("1111", user.Password);
    }

    [Fact]
    async Task UpdateUser_When_User_Doesnt_Exists_Return_NotFound()
    {
        var request = new UserDto
        {
            Id = 864,
            Email = "user@gmail.com",
            Name = "Updated user",
            Surename = "User",
            Username = "user",
            Password = "1111"
        };

        var content = new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PutAsync("user/", content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    async Task DeleteUser_Should_Return_NoContent_204()
    {
        var response = await _client.DeleteAsync("user/3");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    async Task GetUsers_Should_Return_Users()
    {
        var response = await _client.GetAsync("users");
        var json = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<IEnumerable<UserDto>>(json, _jsonSerializerOptions);

        Assert.NotNull(users);
        Assert.True(users.Any());
    }

    private void SeedDatabase()
    {
        var users = new List<User>()
        {
            new()
            {
                Email = "gerry@gmail.com",
                Name = "Gerry",
                Surename = "Ateko",
                Username = "gerriko",
                Password = "123456"
            },
            new()
            {
                Email = "delgado@gmail.com",
                Name = "Anny",
                Surename = "Delgado",
                Username = "anilor",
                Password = "321654"
            },
            new()
            {
                Email = "ggg@gmail.com",
                Name = "Mek",
                Surename = "Aelo",
                Username = "meky",
                Password = "88956"
            },
        };

        _context.Users.AddRange(users);
        _context.SaveChanges();
    }
}
