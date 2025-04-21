using LetsChat.Dtos;
using LetsChat.Intefaces;
using LetsChat.Messages.UpdateMessage;
using LetsChat.Models;
using Moq;

namespace LetsChat.Tests.Unit.Messages;

public class UpdateMessageHandlerTest
{
    Mock<IMessageRepository> _messageRepository;
    UpdateMessageHandler _updateMessageHandler;
    public UpdateMessageHandlerTest()
    {
        _messageRepository = new Mock<IMessageRepository>();
        _updateMessageHandler = new UpdateMessageHandler(_messageRepository.Object);
    }

    [Fact]
    async Task Handle_Should_Return_UpdateMessageResult()
    {
        var request = new MessageDto
        {
            Id = 1,
            Content = "Message updated"
        };

        var message = new Message
        {
            Id = 1,
            Content = "Message updated"
        };

        _messageRepository.Setup(x => x.UpdateMessage(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(message));

        var result = await _updateMessageHandler.Handle(new UpdateMessageRequest(request), CancellationToken.None);

        Assert.NotNull(result.MessageDto);
        Assert.Equal(1, result.MessageDto.Id);
        Assert.Equal("Message updated", result.MessageDto.Content);
    }
}
