namespace LetsChat.Exceptions;

public class MessageNotFoundException : NotFoundException
{
    public MessageNotFoundException(int messageId) : base("Message", messageId)
    {

    }
}
