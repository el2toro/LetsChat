using LetsChat.Auth.Dtos;
using LetsChat.Auth.Services;

namespace LetsChat.Auth.Login;

public record LoginQuery(LoginDto LoginDto) : IRequest<LoginResult>;
public record LoginResult(LoginDto LoginDto);

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.LoginDto).NotNull().WithMessage("LoginDto object is required.");
        RuleFor(x => x.LoginDto.UserName).NotEmpty().WithMessage("UserName is required.");
        RuleFor(x => x.LoginDto.Password).NotEmpty().WithMessage("Password is required.");
    }
}

public class LoginHandler(IAuthenticationRepository authenticationRepository,
    IJwtService jwtService,
    ILogger<LoginHandler> logger)
    : IRequestHandler<LoginQuery, LoginResult>
{
    public async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"LoginHandler called with LoginDto: {request.LoginDto}");

        var user = await authenticationRepository.Login(request.LoginDto) ??
            throw new UserNotFoundException(request.LoginDto.UserId);

        return new LoginResult(MapResult(user));
    }

    private LoginDto MapResult(User user)
    {
        return new LoginDto
        {
            UserId = user.Id,
            FullName = string.Concat(user.Name, " ", user.Surename),
            Token = jwtService.GenerateToken(user.Id, user.Username)
        };
    }
}
