namespace LetsChat.Auth.Dtos;

public class LoginDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Token { get; set; } = default!;
}
