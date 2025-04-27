namespace LetsChat.Repositories;
public class UserRepository(IMessageRepository messageRepository, LetsChatDbContext dbContext, DbContextOptions<LetsChatDbContext> options)
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
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User> GetUserById(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FindAsync(id, cancellationToken) ??
            throw new UserNotFoundException(id);
    }

    public async Task<IEnumerable<UserDto>> GetUsers(int senderId, CancellationToken cancellationToken)
    {
        var users = await dbContext.Users.Select(user => new UserDto
        {
            Username = user.Username,
            Email = user.Email,
            FullName = string.Concat(user.Name, " ", user.Surename),
            Name = user.Name,
            Id = user.Id,
            Password = user.Password,
            Surename = user.Surename
        })
        .AsNoTracking()
        .ToListAsync();

        foreach (var user in users)
        {
            if (user.Id != senderId)
            {
                var lastMessage = await messageRepository.GetLastMessage(senderId, user.Id, cancellationToken);
                user.LastMessage = lastMessage.Content;
            }
        }

        return users;
    }

    public async Task<User> UpdateUser(User user, CancellationToken cancellationToken)
    {
        var existingUser = await dbContext.Users.FindAsync(user.Id) ??
             throw new UserNotFoundException(user.Id);

        existingUser.Surename = user.Surename;
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.Username = user.Username;

        dbContext.Users.Update(existingUser);
        await dbContext.SaveChangesAsync(cancellationToken);

        return existingUser;
    }
}
