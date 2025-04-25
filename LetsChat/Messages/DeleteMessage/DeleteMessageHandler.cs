namespace LetsChat.Messages.DeleteMessage;

public record DeleteMessageRequest(int MessageId) : IRequest<DeleteMessageResult>;
public record DeleteMessageResult(bool IsSuccess);

public class DeleteMessageRequestValidator : AbstractValidator<DeleteMessageRequest>
{
    public DeleteMessageRequestValidator()
    {
        RuleFor(x => x.MessageId).NotEmpty().WithMessage("MessageId is required.");
        RuleFor(x => x.MessageId).GreaterThan(0).WithMessage("MessageId should be greater than 0.");
    }
}

public class DeleteMessageHandler(IMessageRepository messageRepository, ILogger<DeleteMessageHandler> logger)
    : IRequestHandler<DeleteMessageRequest, DeleteMessageResult>
{
    public async Task<DeleteMessageResult> Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteMessageHandler called with MessageId: {MessageId}", request.MessageId);

        await messageRepository.DeleteMessage(request.MessageId, cancellationToken);
        return new DeleteMessageResult(true);
    }
}
