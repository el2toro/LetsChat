namespace LetsChat.Messages.MarkMessageAsRead;

public record MarkMessagesAsReadQuery(int SenderId, int ReceiverId) : IRequest<MarkMessagesAsReadResult>;
public record MarkMessagesAsReadResult(bool IsSuccess);
public class MarkMessagesAsReadHandler(IMessageRepository messageRepository)
    : IRequestHandler<MarkMessagesAsReadQuery, MarkMessagesAsReadResult>
{
    public async Task<MarkMessagesAsReadResult> Handle(MarkMessagesAsReadQuery request, CancellationToken cancellationToken)
    {
        await messageRepository.MarkMessagesAsRead(request.SenderId, request.ReceiverId, cancellationToken);
        return new MarkMessagesAsReadResult(true);
    }
}
