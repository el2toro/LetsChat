namespace LetsChat.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMessages(int senderId, int receiverId);
    Task SendMessage(MessageDto message, CancellationToken cancellationToken);
    Task<Message> GetLastMessage(int senderId, int receiverId, CancellationToken cancellationToken);
    Task MarkMessagesAsRead(int senderId, int receiverId, CancellationToken cancellationToken);
    Task DeleteMessage(int id, CancellationToken cancellationToken);
    Task<Message> UpdateMessage(Message message, CancellationToken cancellationToken);
}
public class MessageRepository(LetsChatDbContext dbContext) : IMessageRepository
{
    public async Task DeleteMessage(int id, CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages.FindAsync(id) ??
            throw new MessageNotFoundException(id);

        dbContext.Messages.Remove(message);
        await dbContext.SaveChangesAsync();
    }

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
        var messagesSender = await dbContext.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId)
            .ToListAsync();

        var messagesReceiver = await dbContext.Messages
            .Where(m => m.ReceiverId == senderId && m.SenderId == receiverId)
            .ToListAsync();

        var concated = messagesSender.Concat(messagesReceiver);

        return messagesSender.Concat(messagesReceiver);
    }

    public async Task MarkMessagesAsRead(int senderId, int receiverId, CancellationToken cancellationToken)
    {
        var messages = await dbContext.Messages
             .Where(m =>
             (m.SenderId == senderId && m.ReceiverId == receiverId && !m.IsRead) ||
             (m.SenderId == receiverId && m.ReceiverId == senderId && !m.IsRead))
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

    public async Task<Message> UpdateMessage(Message message, CancellationToken cancellationToken)
    {
        var existingMessage = await dbContext.Messages.FindAsync(message.Id) ??
            throw new MessageNotFoundException(message.Id);

        existingMessage.Content = message.Content;

        dbContext.Messages.Update(existingMessage);
        await dbContext.SaveChangesAsync();

        return existingMessage;
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
