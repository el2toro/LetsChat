namespace LetsChat.Users.DeleteUser;

public record DeleteUserRequest(int UserId) : IRequest<DeleteUserResult>;
public record DeleteUserResult(bool IsSuccess);
public class DeleteUserHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteUserRequest, DeleteUserResult>
{
    public async Task<DeleteUserResult> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        await userRepository.DeleteUser(request.UserId, cancellationToken);
        return new DeleteUserResult(true);
    }
}
