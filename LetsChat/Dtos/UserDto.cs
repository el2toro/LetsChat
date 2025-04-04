namespace LetsChat.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string LastMessage { get; set; } = default!;
    public string LastMessageSendAt { get; set; } = default!;
    public int MessageCount { get; set; }
}
