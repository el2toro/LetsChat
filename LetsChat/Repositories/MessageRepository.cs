namespace LetsChat.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMessages(int senderId, int receiverId);
    Task SendMessage(MessageDto message, CancellationToken cancellationToken);
}
public class MessageRepository(LetsChatDbContext dbContext) : IMessageRepository
{
    public async Task<IEnumerable<Message>> GetMessages(int senderId, int receiverId)
    {
        var messages = await dbContext.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId)
            .ToListAsync();

        return messages;
    }

    public async Task SendMessage(MessageDto message, CancellationToken cancellationToken)
    {
        var createdMessage = MapMessage(message);

        dbContext.Messages.Add(createdMessage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private Message MapMessage(MessageDto messageDto)
    {
        return new Message
        {
            SenderId = messageDto.SenderId,
            ReceiverId = messageDto.ReceiverId,
            Content = messageDto.Content,
            SendAt = DateTime.Now,
            IsDeleted = messageDto.IsDeleted,
            IsRead = messageDto.IsRead
        };
    }
}
