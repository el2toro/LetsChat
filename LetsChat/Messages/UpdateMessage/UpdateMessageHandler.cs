namespace LetsChat.Messages.UpdateMessage;

public record UpdateMessageRequest(MessageDto MessageDto) : IRequest<UpdateMessageResult>;
public record UpdateMessageResult(MessageDto MessageDto);

public class UpdateMessageHandler(IMessageRepository messageRepository, ILogger<UpdateMessageHandler> logger)
    : IRequestHandler<UpdateMessageRequest, UpdateMessageResult>
{
    public async Task<UpdateMessageResult> Handle(UpdateMessageRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"UpdateMessageHandler called with MessageDto: {request.MessageDto}");

        var result = await messageRepository.UpdateMessage(request.Adapt<Message>(), cancellationToken);

        return new UpdateMessageResult(request.Adapt<MessageDto>());
    }
}
