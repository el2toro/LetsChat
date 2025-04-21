using LetsChat.Dtos;
using LetsChat.Intefaces;
using LetsChat.Models;
using LetsChat.Users.UpdateUser;
using Moq;

namespace LetsChat.Tests.Unit.Users;

public class UpdateUserHandlerTest
{
    Mock<IUserRepository> _userRepository;
    UpdateUserHandler _updateUserHandler;
    public UpdateUserHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _updateUserHandler = new UpdateUserHandler(_userRepository.Object);
    }

    [Fact]
    async Task Handle_Should_Return_UpdateUserResult()
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
            Surname = "Test 2",
            Username = "Testing",
            Id = 1,
            Password = "password",
        };

        _userRepository.Setup(x => x.UpdateUser(It.IsAny<User>(), CancellationToken.None))
            .Returns(Task.FromResult(user));

        var result = await _updateUserHandler.Handle(new UpdateUserRequest(request), CancellationToken.None);

        Assert.IsType<UpdateUserResult>(result);
        Assert.NotNull(result.UserDto);
        Assert.Equal(1, result.UserDto.Id);
    }
}
