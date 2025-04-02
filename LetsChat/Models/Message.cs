namespace LetsChat.Models;

public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; } = default!;
    public DateTime SendAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsRead { get; set; }
    public User Sender { get; set; } = default!;
    public User Receiver { get; set; } = default!;
}
