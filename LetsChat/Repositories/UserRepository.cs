namespace LetsChat.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken);
    Task<User> GetUserById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Message>> GetUserMessagesById(int senderId, int receiverId, CancellationToken cancellationToken);
    Task CreateUser(User user, CancellationToken cancellationToken);
    Task<User> UpdateUser(User user, CancellationToken cancellationToken);
}
public class UserRepository(LetsChatDbContext dbContext) : IUserRepository
{
    public async Task CreateUser(User user, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync(1);
        await dbContext.Users.AddAsync(user,
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

    public async Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken)
    {
        return await dbContext.Users.ToListAsync(cancellationToken);
    }

    public Task<User> UpdateUser(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
