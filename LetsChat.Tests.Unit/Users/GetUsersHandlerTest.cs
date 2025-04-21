using LetsChat.Dtos;
using LetsChat.Intefaces;
using LetsChat.Users.GetUsers;
using Moq;

namespace LetsChat.Tests.Unit.Users;

public class GetUsersHandlerTest
{
    Mock<IUserRepository> _userRepository;
    GetUsersHandler _getUsersHandler;
    public GetUsersHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _getUsersHandler = new GetUsersHandler(_userRepository.Object);
    }

    [Fact]
    async Task Handle_Should_Return_GetUserResult()
    {
        var users = new List<UserDto>
        {
            new UserDto
            {
                Email = "mail",
                Name = "Test",
                Surename = "Test 2",
                Username = "Testing",
                Password = "password",
            },

            new UserDto
            {
                Email = "mail2",
                Name = "Test2",
                Surename = "Test 22",
                Username = "Testing",
                Password = "password",
            }
        };

        _userRepository.Setup(x => x.GetUsers(CancellationToken.None))
            .Returns(Task.FromResult(users.AsEnumerable()));

        var result = await _getUsersHandler.Handle(new GetUsersRequest(), CancellationToken.None);
        Assert.IsType<GetUsersResult>(result);
        Assert.NotNull(result.Users);
        Assert.Equal(2, result.Users.Count());
    }
}
