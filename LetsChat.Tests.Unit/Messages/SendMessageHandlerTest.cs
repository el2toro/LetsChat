using LetsChat.Dtos;
using LetsChat.Intefaces;
using LetsChat.Messages.SendMessage;
using LetsChat.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace LetsChat.Tests.Unit.Messages;

public class SendMessageHandlerTest
{
    Mock<IMessageRepository> _messageRepository;
    Mock<ILogger<SendMessageHandler>> _logger;
    SendMessageHandler _sendMessageHandler;
    public SendMessageHandlerTest()
    {
        _messageRepository = new Mock<IMessageRepository>();
        _logger = new Mock<ILogger<SendMessageHandler>>();
        _sendMessageHandler = new SendMessageHandler(_messageRepository.Object, _logger.Object);
    }

    [Fact]
    async Task Handle_Should_Return_SendMesageResult()
    {
        _messageRepository.Setup(x => x.SendMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var message = new MessageDto
        {
            Content = "new message",
            SendAt = DateTime.UtcNow.ToString(),
            SenderId = 1,
            ReceiverId = 2
        };

        var result = await _sendMessageHandler.Handle(new SendMesageRequest(message), CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }
}
