namespace LetsChat.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Surename { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

