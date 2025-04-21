using LetsChat.Auth.Dtos;

namespace LetsChat.Repositories;
public class AuthenticationRepository(LetsChatDbContext dbContext) : IAuthenticationRepository
{
    public async Task<User> Login(LoginDto loginDto)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == loginDto.UserName && u.Password == loginDto.Password) ??
            throw new NotFoundException("User", loginDto.UserName);

        return user;
    }
}
