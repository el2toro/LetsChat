using LetsChat.Messages.MarkMessageAsRead;
using LetsChat.Models;
using LetsChat.Repositories;
using Moq;

namespace LetsChat.Tests.Unit.Messages;

public class MarkMessageAsReadHandlerTest
{
    Mock<IMessageRepository> _messageRepository;
    MarkMessagesAsReadHandler _markMessagesAsReadHandler;
    public MarkMessageAsReadHandlerTest()
    {
        _messageRepository = new Mock<IMessageRepository>();
        _markMessagesAsReadHandler = new MarkMessagesAsReadHandler(_messageRepository.Object);
    }

    [Fact]
    async Task Handle_Should_Return_MarkMessageAsReadResult()
    {
        var messages = new List<Message>
        {
            new Message
            {
                 Content = "first message",
                 SenderId = 1,
                 ReceiverId = 2,
                 SendAt = DateTime.UtcNow,
                 IsRead = true
            },
            new Message
            {
                 Content = "latest message",
                 SenderId = 2,
                 ReceiverId = 1,
                 SendAt = DateTime.UtcNow,
                 IsRead = true
            }
        };

        _messageRepository.Setup(x => x.MarkMessagesAsRead(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(messages.AsEnumerable()));

        var result = await _markMessagesAsReadHandler.Handle(new MarkMessagesAsReadQuery(1, 2), CancellationToken.None);

        Assert.NotNull(result.Messages);
        Assert.True(result.Messages.Any());

        foreach (var message in result.Messages)
        {
            Assert.True(message.IsRead);
        }
    }
}
