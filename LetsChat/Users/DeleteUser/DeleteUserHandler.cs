namespace LetsChat.Users.DeleteUser;

public record DeleteUserRequest(int UserId) : IRequest<DeleteUserResult>;
public record DeleteUserResult(bool IsSuccess);

public class DeleteUserRequestVAlidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserRequestVAlidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0");
    }
}
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
