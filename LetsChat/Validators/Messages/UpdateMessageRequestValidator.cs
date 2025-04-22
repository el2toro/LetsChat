using LetsChat.Messages.UpdateMessage;

namespace LetsChat.Validators.Messages;

public class UpdateMessageRequestValidator : AbstractValidator<UpdateMessageRequest>
{
    public UpdateMessageRequestValidator()
    {
        RuleFor(x => x.MessageDto.Id)
            .GreaterThan(0)
            .WithMessage("Id should be greater than 0");

        RuleFor(x => x.MessageDto.Content)
            .NotNull()
            .NotEmpty()
            .WithMessage("Message Content cannot be null or empty space");

        RuleFor(x => x.MessageDto.SenderId)
            .GreaterThan(0)
            .WithMessage("SenderId should be greater than 0");

        RuleFor(x => x.MessageDto.ReceiverId)
           .GreaterThan(0)
           .WithMessage("ReceiverId should be greater than 0");
    }
}
