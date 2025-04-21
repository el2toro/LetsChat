using LetsChat.Intefaces;
using LetsChat.Users.DeleteUser;
using Moq;

namespace LetsChat.Tests.Unit.Users;

public class DeleteUserHandlerTest
{
    Mock<IUserRepository> _userRepository;
    DeleteUserHandler _deleteUserHandler;
    public DeleteUserHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _deleteUserHandler = new DeleteUserHandler(_userRepository.Object);
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
