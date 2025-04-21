namespace LetsChat.Users.CreateUser;

public record CreateUserRequest(UserDto UserDto) : IRequest<CreateUserResult>;
public record CreateUserResult(bool IsSuccess);

public class CreateUserHandler(IUserRepository userRepository, ILogger<CreateUserHandler> logger)
    : IRequestHandler<CreateUserRequest, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateUserHandler called with UserDto: {@UserDto}", request.UserDto);

        var user = request.UserDto.Adapt<User>();

        await userRepository.CreateUser(user, cancellationToken);
        return new CreateUserResult(true);
    }
}
