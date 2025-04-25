namespace LetsChat.Messages.GetLastMessage;

public record GetLastMessageRequest(int SenderId, int ReceiverId) : IRequest<GetLastMessageResult>;
public record GetLastMessageResult(MessageDto Message);

public class GetLastMessageHandler(IMessageRepository messageRepository, ILogger<GetLastMessageHandler> logger)
    : IRequestHandler<GetLastMessageRequest, GetLastMessageResult>
{
    public async Task<GetLastMessageResult> Handle(GetLastMessageRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"GetLastMessageHandler called with SenderId: {request.SenderId} and ReceiverId: {request.ReceiverId}");

        var message = await messageRepository.GetLastMessage(request.SenderId, request.ReceiverId, cancellationToken);
        return new GetLastMessageResult(message.Adapt<MessageDto>());
    }
}
