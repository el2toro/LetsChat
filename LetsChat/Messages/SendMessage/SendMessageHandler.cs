namespace LetsChat.Messages.SendMessage;

public record SendMesageRequest(MessageDto Message) : IRequest<SendMesageResult>;
public record SendMesageResult(bool IsSuccess);
public class SendMessageHandler(IMessageRepository messageRepository)
    : IRequestHandler<SendMesageRequest, SendMesageResult>
{
    public async Task<SendMesageResult> Handle(SendMesageRequest request, CancellationToken cancellationToken)
    {
        await messageRepository.SendMessage(request.Message, cancellationToken);
        return new SendMesageResult(true);
    }
}

