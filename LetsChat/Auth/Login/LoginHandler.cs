using LetsChat.Auth.Dtos;
using LetsChat.Auth.Services;

namespace LetsChat.Auth.Login;

public record LoginQuery(LoginDto LoginDto) : IRequest<LoginResult>;
public record LoginResult(LoginDto LoginDto);

public class LoginHandler(IAuthenticationRepository authenticationRepository, IJwtService jwtService)
    : IRequestHandler<LoginQuery, LoginResult>
{
    public async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await authenticationRepository.Login(request.LoginDto) ??
            throw new UserNotFoundException(request.LoginDto.UserId);

        return new LoginResult(MapResult(user));
    }

    private LoginDto MapResult(User user)
    {
        return new LoginDto
        {
            UserId = user.Id,
            FullName = string.Concat(user.Name, " ", user.Surname),
            Token = jwtService.GenerateToken(user.Id, user.Username)
        };
    }
}
