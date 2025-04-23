using LetsChat.Intefaces;
using LetsChat.Messages.GetMessages;
using LetsChat.Models;
using Moq;

namespace LetsChat.Tests.Unit.Messages;

public class GetMessagesHandlerTest
{
    Mock<IMessageRepository> _messageRepository;
    GetMessagesHandler _getMessagesHandler;
    public GetMessagesHandlerTest()
    {
        _messageRepository = new Mock<IMessageRepository>();
        _getMessagesHandler = new GetMessagesHandler(_messageRepository.Object);
    }

    [Fact]
    async Task Handle_Should_Return_GetMessagesResult()
    {
        var messages = new List<Message>
        {
            new Message
            {
                Id = 1,
                Content = "new message",
                SendAt = DateTime.UtcNow.AddDays(-1),
                SenderId = 1,
                ReceiverId = 2,
            },
            new Message
            {
                Id = 2,
                Content = "one more message",
                SendAt = DateTime.UtcNow.AddDays(-1),
                SenderId = 2,
                ReceiverId = 1,
            },
        };

        _messageRepository.Setup(x => x.GetMessages(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(messages.AsEnumerable()));

        var result = await _getMessagesHandler.Handle(new GetMessagesRequest(1, 2), CancellationToken.None);

        Assert.NotNull(result.Messages);
        Assert.Equal(2, result.Messages.Count());
        Assert.Equal(1, result.Messages.First().Id);
        Assert.Equal(2, result.Messages.Last().Id);
    }
}
