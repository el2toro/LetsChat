using LetsChat.Messages.MarkMessageAsRead;
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
        _messageRepository.Setup(x => x.MarkMessagesAsRead(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

       var result = await _markMessagesAsReadHandler.Handle(new MarkMessagesAsReadQuery(1, 2), CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }
}
