namespace LetsChat.Intefaces;

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetUsers(CancellationToken cancellationToken);
    Task<User> GetUserById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Message>> GetUserMessagesById(int senderId, int receiverId, CancellationToken cancellationToken);
    Task CreateUser(User user, CancellationToken cancellationToken);
    Task<User> UpdateUser(User user, CancellationToken cancellationToken);
    Task DeleteUser(int id, CancellationToken cancellationToken);
}
