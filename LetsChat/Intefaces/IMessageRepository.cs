namespace LetsChat.Intefaces;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMessages(int senderId, int receiverId);
    Task SendMessage(MessageDto message, CancellationToken cancellationToken);
    Task<Message> GetLastMessage(int senderId, int receiverId, CancellationToken cancellationToken);
    Task<IEnumerable<Message>> MarkMessagesAsRead(int senderId, int receiverId, CancellationToken cancellationToken);
    Task DeleteMessage(int id, CancellationToken cancellationToken);
    Task<Message> UpdateMessage(Message message, CancellationToken cancellationToken);
}
