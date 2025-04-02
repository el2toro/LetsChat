namespace LetsChat.Users.GetUserMessagesById;

public record GetUserMessagesByIdRequest(int Id, int WithUserId) : IRequest<GetUserMessagesByIdResult>;
public record GetUserMessagesByIdResult(IEnumerable<Message> Messages);
public class GetUserMessagesByIdHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserMessagesByIdRequest, GetUserMessagesByIdResult>
{
    public async Task<GetUserMessagesByIdResult> Handle(GetUserMessagesByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetUserMessagesById(request.Id, request.WithUserId, cancellationToken);
        return new GetUserMessagesByIdResult(result);
    }
}
