using LetsChat.Messages.GetMessages;

public class GetMessagesRequestValidator : AbstractValidator<GetMessagesRequest>
{
    public GetMessagesRequestValidator()
    {
        RuleFor(x => x.SenderId).NotEmpty().WithMessage("SenderId is required.");
        RuleFor(x => x.SenderId).GreaterThan(0).WithMessage("SenderId should be greater than 0.");

        RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("ReceiverId is required.");
        RuleFor(x => x.ReceiverId).GreaterThan(0).WithMessage("ReceiverId should be greater than 0.");
    }
}