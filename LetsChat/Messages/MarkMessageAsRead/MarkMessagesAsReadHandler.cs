namespace LetsChat.Messages.MarkMessageAsRead;

public record MarkMessagesAsReadQuery(int SenderId, int ReceiverId) : IRequest<MarkMessagesAsReadResult>;
public record MarkMessagesAsReadResult(IEnumerable<Message> Messages);
public class MarkMessagesAsReadHandler(IMessageRepository messageRepository)
    : IRequestHandler<MarkMessagesAsReadQuery, MarkMessagesAsReadResult>
{
    public async Task<MarkMessagesAsReadResult> Handle(MarkMessagesAsReadQuery request, CancellationToken cancellationToken)
    {
        var messages = await messageRepository.MarkMessagesAsRead(request.SenderId, request.ReceiverId, cancellationToken);
        return new MarkMessagesAsReadResult(messages);
    }
}
