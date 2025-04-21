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
        return new GetUserByIdResult(MapToDto(user));
    }

    //TODO: use automapper
    private UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        FullName = string.Concat(user.Name, " ", user.Surname),
        Surename = user.Surname,
        Username = user.Username,
        Password = user.Password
    };
}
