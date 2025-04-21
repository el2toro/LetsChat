namespace LetsChat.Repositories;
public class UserRepository(LetsChatDbContext dbContext, DbContextOptions<LetsChatDbContext> options)
    : IUserRepository
{
    public async Task CreateUser(User user, CancellationToken cancellationToken)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUser(int id, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync(id, cancellationToken) ??
            throw new UserNotFoundException(id);

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<User> GetUserById(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FindAsync(id, cancellationToken) ??
            throw new UserNotFoundException(id);
    }

    public async Task<IEnumerable<Message>> GetUserMessagesById(int senderId, int receiverId, CancellationToken cancellationToken)
    {
        return await dbContext.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId && !m.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserDto>> GetUsers(CancellationToken cancellationToken)
    {
        return await dbContext.Users.Select(user => new UserDto
        {
            Username = user.Username,
            Email = user.Email,
            FullName = string.Concat(user.Name, " ", user.Surname),
            Name = user.Name,
            Id = user.Id,
            Password = user.Password,
            Surename = user.Surname,

        }).ToListAsync();
    }

    public async Task<User> UpdateUser(User user, CancellationToken cancellationToken)
    {
        var existingUser = await dbContext.Users.FindAsync(user.Id) ??
             throw new UserNotFoundException(user.Id);

        existingUser.Surname = user.Surname;
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.Username = user.Username;

        dbContext.Users.Update(existingUser);
        await dbContext.SaveChangesAsync();

        return existingUser;
    }

    private async Task<(string content, string sentAt, int count)> GetMessageDetails(int senderId, int receiverId)
    {
        using LetsChatDbContext context = new LetsChatDbContext(options);

        var query = context.Messages
            .OrderBy(m => m.SendAt)
            .Where(m =>
                m.SenderId == senderId && m.ReceiverId == receiverId ||
                m.SenderId == receiverId && m.ReceiverId == senderId);

        var message = await query.LastOrDefaultAsync();
        var unreadMessages = await context.Messages
            .Where(m => m.SenderId == receiverId && m.ReceiverId == senderId && !m.IsRead)
            .CountAsync();


        return (message?.Content, message?.SendAt.ToString(), unreadMessages);
    }
}
