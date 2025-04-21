namespace LetsChat.Users.DeleteUser;

public record DeleteUserRequest(int UserId) : IRequest<DeleteUserResult>;
public record DeleteUserResult(bool IsSuccess);
public class DeleteUserHandler(IUserRepository userRepository, ILogger<DeleteUserHandler> logger)
    : IRequestHandler<DeleteUserRequest, DeleteUserResult>
{
    public async Task<DeleteUserResult> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteUserHandler called with UserId: {UserId}", request.UserId);

        await userRepository.DeleteUser(request.UserId, cancellationToken);
        return new DeleteUserResult(true);
    }
}
