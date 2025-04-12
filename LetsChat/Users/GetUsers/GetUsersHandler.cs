namespace LetsChat.Users.GetUsers;

public record GetUsersRequest() : IRequest<GetUsersResult>;
public record GetUsersResult(IEnumerable<UserDto> Users);
public class GetUsersHandler(IUserRepository userRepository) : IRequestHandler<GetUsersRequest, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsers(cancellationToken);
        return new GetUsersResult(users);
    }
}
