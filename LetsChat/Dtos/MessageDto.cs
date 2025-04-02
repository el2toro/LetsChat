namespace LetsChat.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; } = default!;
    public string SendAt { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public bool IsRead { get; set; }
}
