namespace LetsChat.Messages.GetMessages;

public record GetMessagesRequest(int SenderId, int ReceiverId) : IRequest<GetMessagesResult>;
public record GetMessagesResult(IEnumerable<Message> Messages);

public class GetMessagesHandler(IMessageRepository messageRepository, ILogger<GetMessagesHandler> logger)
    : IRequestHandler<GetMessagesRequest, GetMessagesResult>
{
    public async Task<GetMessagesResult> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"GetMessagesHandler called with SenderId: {request.SenderId} and ReceiverId: {request.ReceiverId}");

        var result = await messageRepository.GetMessages(request.SenderId, request.ReceiverId, cancellationToken);
        return new GetMessagesResult(result);
    }
}
