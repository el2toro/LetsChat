using LetsChat.Auth.Dtos;

namespace LetsChat.Repositories;

public interface IAuthenticationRepository
{
    Task<User> Login(LoginDto loginDto);
    Task Logout();
}
public class AuthenticationRepository(LetsChatDbContext dbContext) : IAuthenticationRepository
{
    public async Task<User> Login(LoginDto loginDto)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == loginDto.UserName && u.Password == loginDto.Password);

        return user;
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }
}
