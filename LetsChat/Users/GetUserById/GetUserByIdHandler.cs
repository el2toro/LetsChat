namespace LetsChat.Users.GetUserById;

public record GetUserByIdRequest(int UserId) : IRequest<GetUserByIdResult>;
public record GetUserByIdResult(UserDto UserDto);

public class GetUserByIdHandler(IUserRepository userRepository, ILogger<GetUserByIdHandler> logger)
    : IRequestHandler<GetUserByIdRequest, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetUserByIdHandler called with UserId: {UserId}", request.UserId);

        var user = await userRepository.GetUserById(request.UserId, cancellationToken);

        var result = user.Adapt<UserDto>();

        return new GetUserByIdResult(result);
    }
}
