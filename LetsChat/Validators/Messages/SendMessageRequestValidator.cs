using LetsChat.Messages.SendMessage;

namespace LetsChat.Validators.Messages;

public class SendMessageRequestValidator : AbstractValidator<SendMesageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotNull()
            .WithMessage("Message cannot be null");

        RuleFor(x => x.Message.SenderId)
            .GreaterThan(0)
            .WithMessage("SenderId should be greater than 0");

        RuleFor(x => x.Message.ReceiverId)
            .GreaterThan(0)
            .WithMessage("ReceiverId should be greater than 0");

        RuleFor(x => x.Message.Content)
            .NotNull()
            .NotEmpty()
            .WithMessage("Message Content cannot be null or empty space");
    }
}
