namespace LetsChat.Messages.MarkMessageAsRead;

public record MarkMessagesAsReadQuery(int SenderId, int ReceiverId) : IRequest<MarkMessagesAsReadResult>;
public record MarkMessagesAsReadResult(IEnumerable<Message> Messages);

public class MarkMessagesAsReadHandler(IMessageRepository messageRepository, ILogger<MarkMessagesAsReadHandler> logger)
    : IRequestHandler<MarkMessagesAsReadQuery, MarkMessagesAsReadResult>
{
    public async Task<MarkMessagesAsReadResult> Handle(MarkMessagesAsReadQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"MarkMessagesAsReadHandler called with SenderId: {request.SenderId} and ReceiverId: {request.ReceiverId}");

        var messages = await messageRepository.MarkMessagesAsRead(request.SenderId, request.ReceiverId, cancellationToken);
        return new MarkMessagesAsReadResult(messages);
    }
}
