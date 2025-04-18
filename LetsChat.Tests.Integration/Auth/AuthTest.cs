﻿using LetsChat.Auth.Dtos;
using LetsChat.Data;
using LetsChat.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LetsChat.Tests.Integration.Auth;

public class AuthTest : IClassFixture<CustomWebAppFactoryIntegrationTest>
{
    private readonly HttpClient _client;
    private readonly LetsChatDbContext _context;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public AuthTest(CustomWebAppFactoryIntegrationTest factory)
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _client = factory.CreateClient();
        _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<LetsChatDbContext>();

        SeedUsers();
    }

    [Fact]
    async Task Login_Should_Return_LoginDto()
    {
        var request = new LoginDto
        {
            UserName = "testing",
            Password = "password"
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("login/", content);
        var json = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<LoginDto>(json, _jsonSerializerOptions);

        Assert.NotNull(user);
        Assert.Equal(1, user.UserId);
        Assert.Equal("Test Test", user.FullName);
        Assert.NotEmpty(user.Token);
        Assert.Null(user.UserName);
        Assert.Null(user.Password);
    }

    [Fact]
    async Task Login_When_User_Not_Exists_Return_NotFound()
    {
        var request = new LoginDto
        {
            UserName = "badtesting",
            Password = "password"
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("login/", content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private void SeedUsers()
    {
        var users = new List<User>
        {
            new()
            {
                Username = "testing",
                Password = "password",
                Name = "Test",
                Surname = "Test",
                Email = "test@gmail.com",
            },
            new()
            {
                Username = "testing2",
                Password = "password2",
                Name = "Test2",
                Surname = "Test2",
                Email = "test2@gmail.com",
            },
        };

        _context.Users.AddRange(users);
        _context.SaveChanges();
    }
}
