using LetsChat.Dtos;
using LetsChat.Intefaces;
using LetsChat.Models;
using LetsChat.Users.CreateUser;
using Microsoft.Extensions.Logging;
using Moq;

namespace LetsChat.Tests.Unit.Users;

public class CreateUserHandlerTest
{
    CreateUserHandler sut;
    Mock<IUserRepository> _userRepository;
    Mock<ILogger<CreateUserHandler>> _logger;
    public CreateUserHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _logger = new Mock<ILogger<CreateUserHandler>>();
        sut = new CreateUserHandler(_userRepository.Object, _logger.Object);
    }

    [Fact]
    async void Handle_Should_Return_CreateUserResult()
    {
        var request = new UserDto
        {
            Email = "mail",
            Name = "Test",
            Surename = "Test 2",
            Username = "Testing",
            Password = "password",
        };

        var user = new User
        {
            Email = "mail",
            Name = "Test",
            Surename = "Test 2",
            Username = "Testing",
            Id = 1,
            Password = "password",
        };

        var cancellationToken = new CancellationToken();

        _userRepository.Setup(c => c.CreateUser(user, cancellationToken));

        var result = await sut.Handle(new CreateUserRequest(request), cancellationToken);
        Assert.IsType<CreateUserResult>(result);
        Assert.True(result.IsSuccess);
    }
}
