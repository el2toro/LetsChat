namespace LetsChat.Messages.SendMessage;

public record SendMesageRequest(MessageDto Message) : IRequest<SendMesageResult>;
public record SendMesageResult(bool IsSuccess);
public class SendMessageHandler(IMessageRepository messageRepository)
    : IRequestHandler<SendMesageRequest, SendMesageResult>
{
    public async Task<SendMesageResult> Handle(SendMesageRequest request, CancellationToken cancellationToken)
    {
        await messageRepository.SendMessage(MapDtoToMessage(request.Message), cancellationToken);
        return new SendMesageResult(true);
    }

    private Message MapDtoToMessage(MessageDto messageDto)
    {
        return new Message
        {
            SenderId = messageDto.SenderId,
            ReceiverId = messageDto.ReceiverId,
            Content = messageDto.Content,
            SendAt = DateTime.Now,
            IsDeleted = messageDto.IsDeleted,
            IsRead = messageDto.IsRead
        };
    }
}

