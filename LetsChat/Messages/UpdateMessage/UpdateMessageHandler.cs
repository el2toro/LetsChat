namespace LetsChat.Messages.UpdateMessage;

public record UpdateMessageRequest(MessageDto MessageDto) : IRequest<UpdateMessageResult>;
public record UpdateMessageResult(MessageDto MessageDto);
public class UpdateMessageHandler(IMessageRepository messageRepository)
    : IRequestHandler<UpdateMessageRequest, UpdateMessageResult>
{
    public async Task<UpdateMessageResult> Handle(UpdateMessageRequest request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            Id = request.MessageDto.Id,
            Content = request.MessageDto.Content
        };

        var result = await messageRepository.UpdateMessage(message, cancellationToken);

        //TODO: add proper mapping
        request.MessageDto.Content = request.MessageDto.Content;

        return new UpdateMessageResult(request.MessageDto);
    }
}
