using LetsChat.Intefaces;
using LetsChat.Users.DeleteUser;
using Microsoft.Extensions.Logging;
using Moq;

namespace LetsChat.Tests.Unit.Users;

public class DeleteUserHandlerTest
{
    Mock<IUserRepository> _userRepository;
    Mock<ILogger<DeleteUserHandler>> _logger;
    DeleteUserHandler _deleteUserHandler;
    public DeleteUserHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _logger = new Mock<ILogger<DeleteUserHandler>>();
        _deleteUserHandler = new DeleteUserHandler(_userRepository.Object, _logger.Object);
    }

    [Fact]
    async Task Handle_Should_Return_DeleteUserResult()
    {
        var request = new DeleteUserRequest(1);

        _userRepository.Setup(x => x.DeleteUser(1, CancellationToken.None));
        var result = await _deleteUserHandler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }
}
