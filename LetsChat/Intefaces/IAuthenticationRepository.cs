using LetsChat.Auth.Dtos;

namespace LetsChat.Intefaces;

public interface IAuthenticationRepository
{
    Task<User> Login(LoginDto loginDto);
}
