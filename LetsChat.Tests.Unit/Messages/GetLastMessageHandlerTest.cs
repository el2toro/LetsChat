using LetsChat.Intefaces;
using LetsChat.Messages.GetLastMessage;
using LetsChat.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace LetsChat.Tests.Unit.Messages;

public class GetLastMessageHandlerTest
{
    Mock<IMessageRepository> _messageRepository;
    Mock<ILogger<GetLastMessageHandler>> _logger;
    GetLastMessageHandler _getLastMessageHandler;
    public GetLastMessageHandlerTest()
    {
        _messageRepository = new Mock<IMessageRepository>();
        _logger = new Mock<ILogger<GetLastMessageHandler>>();
        _getLastMessageHandler = new GetLastMessageHandler(_messageRepository.Object, _logger.Object);
    }

    [Fact]
    async Task Handle_Should_Return_GetLastMessageResult()
    {
        var message = new Message
        {
            Id = 1,
            Content = "new message",
            SendAt = DateTime.UtcNow.AddDays(-1),
            SenderId = 1,
            ReceiverId = 2,
        };

        _messageRepository.Setup(x => x.GetLastMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(message));

        var result = await _getLastMessageHandler.Handle(new GetLastMessageRequest(1, 2), CancellationToken.None);

        Assert.NotNull(result.Message);
        Assert.Equal(1, result.Message.Id);
        Assert.Equal(1, result.Message.SenderId);
        Assert.Equal(2, result.Message.ReceiverId);
    }
}
