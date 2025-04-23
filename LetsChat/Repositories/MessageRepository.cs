namespace LetsChat.Repositories;
public class MessageRepository(LetsChatDbContext dbContext)
    : IMessageRepository
{
    public async Task DeleteMessage(int id, CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages.FindAsync(id) ??
            throw new MessageNotFoundException(id);

        dbContext.Messages.Remove(message);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Message> GetLastMessage(int senderId, int receiverId, CancellationToken cancellationToken)
    {
        return await dbContext.Messages
            .AsNoTracking()
            .OrderBy(m => m.SendAt)
            .LastOrDefaultAsync(m => m.SenderId == senderId && m.ReceiverId == receiverId ||
                m.SenderId == receiverId && m.ReceiverId == senderId, cancellationToken: cancellationToken) ??
                throw new ArgumentNullException();
    }

    public async Task<IEnumerable<Message>> GetMessages(int senderId, int receiverId, CancellationToken cancellationToken)
    {
        var messagesSender = await dbContext.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var messagesReceiver = await dbContext.Messages
            .Where(m => m.ReceiverId == senderId && m.SenderId == receiverId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var concated = messagesSender.Concat(messagesReceiver);

        return messagesSender.Concat(messagesReceiver);
    }

    public async Task<IEnumerable<Message>> MarkMessagesAsRead(int senderId, int receiverId, CancellationToken cancellationToken)
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

        return messages;
    }

    public async Task SendMessage(Message message, CancellationToken cancellationToken)
    {
        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Message> UpdateMessage(Message message, CancellationToken cancellationToken)
    {
        var existingMessage = await dbContext.Messages.FindAsync(message.Id) ??
            throw new MessageNotFoundException(message.Id);

        existingMessage.Content = message.Content;

        dbContext.Messages.Update(existingMessage);
        await dbContext.SaveChangesAsync(cancellationToken);

        return existingMessage;
    }
}
