using LetsChat.Dtos;
using LetsChat.Intefaces;
using LetsChat.Messages.SendMessage;
using Moq;

namespace LetsChat.Tests.Unit.Messages;

public class SendMessageHandlerTest
{
    Mock<IMessageRepository> _messageRepository;
    SendMessageHandler _sendMessageHandler;
    public SendMessageHandlerTest()
    {
        _messageRepository = new Mock<IMessageRepository>();
        _sendMessageHandler = new SendMessageHandler(_messageRepository.Object);
    }

    [Fact]
    async Task Handle_Should_Return_SendMesageResult()
    {
        _messageRepository.Setup(x => x.SendMessage(It.IsAny<MessageDto>(), It.IsAny<CancellationToken>()))
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
