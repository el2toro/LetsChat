using LetsChat.Intefaces;
using LetsChat.Messages.DeleteMessage;
using Moq;

namespace LetsChat.Tests.Unit.Messages;

public class DeleteMessageHandlerTest
{
    Mock<IMessageRepository> _messageRepository;
    DeleteMessageHandler _deleteMessageHandler;
    public DeleteMessageHandlerTest()
    {
        _messageRepository = new Mock<IMessageRepository>();
        _deleteMessageHandler = new DeleteMessageHandler(_messageRepository.Object);
    }

    [Fact]
    async Task DeleteMessageHandler_Should_Return_DeleteMessageResult()
    {
        _messageRepository.Setup(x => x.DeleteMessage(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _deleteMessageHandler.Handle(new DeleteMessageRequest(1), CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }
}
