namespace LetsChat.Messages.GetMessages;

public record GetMessagesRequest(int SenderId, int ReceiverId) : IRequest<GetMessagesResult>;
public record GetMessagesResult(IEnumerable<Message> Messages);

public class GetMessagesHandler(IMessageRepository messageRepository)
    : IRequestHandler<GetMessagesRequest, GetMessagesResult>
{
    public async Task<GetMessagesResult> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
    {
        var result = await messageRepository.GetMessages(request.SenderId, request.ReceiverId, cancellationToken);
        return new GetMessagesResult(result);
    }
}
