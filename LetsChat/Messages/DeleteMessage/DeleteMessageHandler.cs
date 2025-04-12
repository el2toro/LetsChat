namespace LetsChat.Messages.DeleteMessage;

public record DeleteMessageRequest(int MessageId) : IRequest<DeleteMessageResult>;
public record DeleteMessageResult(bool IsSuccess);
public class DeleteMessageHandler(IMessageRepository messageRepository)
    : IRequestHandler<DeleteMessageRequest, DeleteMessageResult>
{
    public async Task<DeleteMessageResult> Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        await messageRepository.DeleteMessage(request.MessageId, cancellationToken);
        return new DeleteMessageResult(true);
    }
}
