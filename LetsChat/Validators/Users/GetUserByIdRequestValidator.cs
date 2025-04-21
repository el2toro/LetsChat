using LetsChat.Users.GetUserById;

namespace LetsChat.Validators.Users;

public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdRequest>
{
    public GetUserByIdRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId should be greater than 0");
    }
}