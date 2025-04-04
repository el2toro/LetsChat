namespace LetsChat.Users.GetUsers;

public record GetUserRequest(int SenderId) : IRequest<GetUserResponse>;
public record GetUserResponse(IEnumerable<UserDto> Users);
public class GetUsersHandler(IUserRepository userRepository) : IRequestHandler<GetUserRequest, GetUserResponse>
{
    public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsers(request.SenderId, cancellationToken);
        return new GetUserResponse(users);
    }
}
