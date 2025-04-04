namespace LetsChat.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMessages(int senderId, int receiverId);
    Task SendMessage(MessageDto message, CancellationToken cancellationToken);
    Task<Message> GetLastMessage(int senderId, int receiverId, CancellationToken cancellationToken);
    Task MarkMessagesAsRead(int senderId, int receiverId, CancellationToken cancellationToken);
}
public class MessageRepository(LetsChatDbContext dbContext) : IMessageRepository
{
    public async Task<Message> GetLastMessage(int senderId, int receiverId, CancellationToken cancellationToken)
    {
        return await dbContext.Messages
            .OrderBy(m => m.SendAt)
            .LastOrDefaultAsync(m => m.SenderId == senderId && m.ReceiverId == receiverId ||
                m.SenderId == receiverId && m.ReceiverId == senderId) ??
                throw new ArgumentNullException();
    }

    public async Task<IEnumerable<Message>> GetMessages(int senderId, int receiverId)
    {
        var messages = await dbContext.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId)
            .ToListAsync();

        return messages;
    }

    public async Task MarkMessagesAsRead(int senderId, int receiverId, CancellationToken cancellationToken)
    {
        var messages = await dbContext.Messages
             .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId && !m.IsRead)
             .ToListAsync();

        foreach (var message in messages)
        {
            message.IsRead = true;
        }
        await dbContext.SaveChangesAsync(cancellationToken);
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
