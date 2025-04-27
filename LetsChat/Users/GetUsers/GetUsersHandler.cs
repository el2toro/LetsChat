namespace LetsChat.Users.GetUsers;

public record GetUsersRequest(int SenderId) : IRequest<GetUsersResult>;
public record GetUsersResult(IEnumerable<UserDto> Users);
public class GetUsersHandler(IUserRepository userRepository, ILogger<GetUsersHandler> logger)
    : IRequestHandler<GetUsersRequest, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetUsersHandler called");

        var users = await userRepository.GetUsers(request.SenderId, cancellationToken);
        return new GetUsersResult(users);
    }
}
