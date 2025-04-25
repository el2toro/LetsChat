namespace LetsChat.Messages.SendMessage;

public record SendMesageRequest(MessageDto Message) : IRequest<SendMesageResult>;
public record SendMesageResult(bool IsSuccess);
public class SendMessageHandler(IMessageRepository messageRepository, ILogger<SendMessageHandler> logger)
    : IRequestHandler<SendMesageRequest, SendMesageResult>
{
    public async Task<SendMesageResult> Handle(SendMesageRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"SendMessageHandler called with MessageDto: {request.Message}");

        var message = request.Message.Adapt<Message>();

        await messageRepository.SendMessage(message, cancellationToken);
        return new SendMesageResult(true);
    }
}

