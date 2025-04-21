namespace LetsChat.Users.UpdateUser;

public record UpdateUserRequest(UserDto UserDto) : IRequest<UpdateUserResult>;
public record UpdateUserResult(UserDto UserDto);
public class UpdateUserHandler(IUserRepository userRepository, ILogger<UpdateUserHandler> logger)
    : IRequestHandler<UpdateUserRequest, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateUserHandler called with UserDto: {UserDto}", request.UserDto);

        var user = request.UserDto.Adapt<User>();

        var updatedUser = await userRepository.UpdateUser(user, cancellationToken);
        return new UpdateUserResult(updatedUser.Adapt<UserDto>());
    }
}
