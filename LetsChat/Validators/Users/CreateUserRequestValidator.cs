using LetsChat.Users.CreateUser;

namespace LetsChat.Validators.Users;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        string message = "is required and should have at least 4 characters";

        RuleFor(x => x.UserDto)
            .NotNull()
            .WithMessage("UserDto object is required");

        RuleFor(x => x.UserDto.Username)
            .NotNull()
            .MinimumLength(4)
            .WithMessage($"Username {message}");

        RuleFor(x => x.UserDto.Name)
             .NotNull()
             .MinimumLength(4)
             .WithMessage($"Name {message}");

        RuleFor(x => x.UserDto.Surename)
            .NotNull()
            .MinimumLength(4)
            .WithMessage($"Surename {message}");

        RuleFor(x => x.UserDto.Email)
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
            .WithMessage("Email is not valid");

        RuleFor(x => x.UserDto.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$")
            .WithMessage("Password must contain at least 8 characters, one uppercase letter, one lowercase letter, and one number");
    }
}