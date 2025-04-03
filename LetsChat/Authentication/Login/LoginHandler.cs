using LetsChat.Authentication.Dtos;

namespace LetsChat.Authentication.Login;

public record LoginQuery(LoginDto LoginDto) : IRequest<LoginResult>;
public record LoginResult(User User);

public class LoginHandler(IAuthenticationRepository authenticationRepository)
    : IRequestHandler<LoginQuery, LoginResult>
{
    public async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await authenticationRepository.Login(request.LoginDto);
        return new LoginResult(user);
    }
}
