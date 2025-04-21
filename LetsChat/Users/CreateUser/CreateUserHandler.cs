using LetsChat.Intefaces;

namespace LetsChat.Users.CreateUser;

public record CreateUserRequest(UserDto UserDto) : IRequest<CreateUserResult>;
public record CreateUserResult(bool IsSuccess);
public class CreateUserHandler(IUserRepository userRepository)
    : IRequestHandler<CreateUserRequest, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        await userRepository.CreateUser(MapUser(request.UserDto), cancellationToken);
        return new CreateUserResult(true);
    }

    private User MapUser(UserDto userDto)
    {
        return new User
        {
            Email = userDto.Email,
            Name = userDto.Name,
            Password = userDto.Password,
            Surname = userDto.Surename,
            Username = userDto.Username
        };
    }
}
