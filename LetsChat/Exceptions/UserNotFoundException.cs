namespace LetsChat.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(int userId) : base("User", userId)
    {
    }
}
