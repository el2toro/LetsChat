using Azure.Messaging;

namespace LetsChat.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetUsers(int senderId, CancellationToken cancellationToken);
    Task<User> GetUserById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Message>> GetUserMessagesById(int senderId, int receiverId, CancellationToken cancellationToken);
    Task CreateUser(User user, CancellationToken cancellationToken);
    Task<User> UpdateUser(User user, CancellationToken cancellationToken);
}
public class UserRepository(LetsChatDbContext dbContext, IConfiguration configuration) : IUserRepository
{
    public async Task CreateUser(User user, CancellationToken cancellationToken)
    {
        //var user = await dbContext.Users.FindAsync(1);
        //await dbContext.Users.AddAsync(user,
    }

    public async Task<User> GetUserById(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FirstAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Message>> GetUserMessagesById(int senderId, int receiverId, CancellationToken cancellationToken)
    {
        return await dbContext.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId && !m.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserDto>> GetUsers(int senderId, CancellationToken cancellationToken)
    {
        var users = await dbContext.Users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            FullName = string.Concat(u.Name, " ", u.Surname),
        }).ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            var (messageContent, sentAt, unreadMessages) = await GetMessageDetails(senderId, user.Id);

            user.LastMessage = messageContent;
            user.LastMessageSendAt = sentAt;
            user.MessageCount = unreadMessages;
        }

        return users;
    }

    public Task<User> UpdateUser(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<(string content, string sentAt, int count)> GetMessageDetails(int senderId, int receiverId)
    {
        using LetsChatDbContext context = new LetsChatDbContext(configuration);

        var query = context.Messages
            .OrderBy(m => m.SendAt)
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId ||
                m.SenderId == receiverId && m.ReceiverId == senderId && !m.IsRead);

        var message = await query.LastOrDefaultAsync();
        var unreadMessages = await context.Messages
            .Where(m => m.SenderId == receiverId && m.ReceiverId == senderId && !m.IsRead)
            .CountAsync();


        return (message?.Content, message?.SendAt.ToString(), unreadMessages);
    }
}
