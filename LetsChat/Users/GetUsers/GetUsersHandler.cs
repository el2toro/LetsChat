namespace LetsChat.Users.GetUsers;

public record GetUserRequest() : IRequest<GetUserResponse>;
public record GetUserResponse(IEnumerable<User> Users);
public class GetUsersHandler(IUserRepository userRepository) : IRequestHandler<GetUserRequest, GetUserResponse>
{
    public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsers(cancellationToken);
        return new GetUserResponse(users);
    }
}
