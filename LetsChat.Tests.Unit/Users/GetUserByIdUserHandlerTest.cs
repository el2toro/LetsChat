﻿using LetsChat.Models;
using LetsChat.Repositories;
using LetsChat.Users.GetUserById;
using Moq;

namespace LetsChat.Tests.Unit.Users;

public class GetUserByIdUserHandlerTest
{
    Mock<IUserRepository> _userRepository;
    GetUserByIdHandler _getUserByIdHandler;
    public GetUserByIdUserHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _getUserByIdHandler = new GetUserByIdHandler(_userRepository.Object);
    }

    [Fact]
    async Task GetUserById_Should_Return_GetUserByIdResult()
    {
        var request = new GetUserByIdRequest(1);

        var user = new User
        {
            Id = 1,
            Email = "newmail@gmail.com",
            Name = "Test",
            Password = "123456",
            Surname = "Test2",
            Username = "Testing",
        };

        _userRepository.Setup(x => x.GetUserById(1, CancellationToken.None))
            .Returns(Task.FromResult(user));

        var result = await _getUserByIdHandler.Handle(request, CancellationToken.None);

        Assert.NotNull(result.UserDto);
        Assert.Equal(1, result.UserDto.Id);
        Assert.Equal("newmail@gmail.com", result.UserDto.Email);
        Assert.Equal("Test", result.UserDto.Name);
    }
}
