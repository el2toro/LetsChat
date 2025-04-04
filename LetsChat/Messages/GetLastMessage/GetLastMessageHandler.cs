namespace LetsChat.Messages.GetLastMessage;

public record GetLastMessageRequest(int SenderId, int ReceiverId) : IRequest<GetLastMessageResult>;
public record GetLastMessageResult(MessageDto Message);
public class GetLastMessageHandler(IMessageRepository messageRepository)
    : IRequestHandler<GetLastMessageRequest, GetLastMessageResult>
{
    public async Task<GetLastMessageResult> Handle(GetLastMessageRequest request, CancellationToken cancellationToken)
    {
        var message = await messageRepository.GetLastMessage(request.SenderId, request.ReceiverId, cancellationToken);
        return new GetLastMessageResult(MapToDto(message));
    }

    private MessageDto MapToDto(Message message)
    {
        return new MessageDto
        {
            SenderId = message.SenderId,
            ReceiverId = message.ReceiverId,
            Content = message.Content,
            Id = message.Id,
            IsDeleted = message.IsDeleted,
            IsRead = message.IsRead,
            SendAt = message.SendAt.ToString()
        };
    }
}
