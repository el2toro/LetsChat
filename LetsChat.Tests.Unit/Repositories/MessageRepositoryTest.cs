using LetsChat.Data;
using LetsChat.Exceptions;
using LetsChat.Intefaces;
using LetsChat.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LetsChat.Tests.Unit.Repositories;

public class MessageRepositoryTest : IClassFixture<CustomWebAppFactoryUnitTest>
{
    private readonly LetsChatDbContext _dbContext;
    private readonly IMessageRepository _messageRepository;
    public MessageRepositoryTest(CustomWebAppFactoryUnitTest factory)
    {
        var scope = factory.Services.CreateScope();

        _dbContext = scope.ServiceProvider.GetRequiredService<LetsChatDbContext>();
        _messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();

        AddMessagesToDb();
    }

    [Fact]
    async Task DeleteMessage_Should_Remove_Message_From_Database()
    {
        var dbMessage = _dbContext.Messages.FirstOrDefault();
        await _messageRepository.DeleteMessage(dbMessage.Id, CancellationToken.None);

        var message = _dbContext.Messages.SingleOrDefault(m => m.Id == dbMessage.Id);

        Assert.Null(message);
    }

    [Fact]
    async Task DeleteMessage_When_NoMessage_Throws_MessageNotFoundException()
    {
        var result = await Assert.ThrowsAsync<MessageNotFoundException>(async () =>
                     await _messageRepository.DeleteMessage(9999, CancellationToken.None));

        Assert.NotNull(result);
        Assert.Equal("Entity \"Message\" (9999) was not found.", result.Message);
    }

    [Fact]
    async Task GetMessages_Should_Get_Last_Message_From_Database()
    {
        var messages = await _messageRepository.GetMessages(1, 2, CancellationToken.None);

        Assert.True(messages.Any());
    }

    [Fact]
    async Task MarkMessagesAsRead()
    {
        var result = await _messageRepository.MarkMessagesAsRead(1, 2, CancellationToken.None);

        var messages = _dbContext.Messages
            .Where(m => m.SenderId == 1 && m.ReceiverId == 2)
            .ToList();

        Assert.True(messages.Any());
        foreach (var message in messages)
        {
            Assert.True(message.IsRead);
        }
    }

    [Fact]
    async Task SendMessage_Should_Add_Message_To_Database()
    {
        var message = new Message
        {
            SenderId = 1,
            ReceiverId = 2,
            Content = "Hello hello test",
            SendAt = DateTime.UtcNow
        };

        await _messageRepository.SendMessage(message, CancellationToken.None);

        var dbMessage = _dbContext.Messages
            .SingleOrDefault(m => m.Content == message.Content);

        Assert.NotNull(dbMessage);
        Assert.Equal(message.Content, dbMessage.Content);
    }

    [Fact]
    async Task UpdateMessage_Should_Update_Message_InTo_Database()
    {
        var messageToUpdate = new Message
        {
            Id = 1,
            Content = "Hello hello test updated"
        };

        var updatedMessage = await _messageRepository.UpdateMessage(messageToUpdate, CancellationToken.None);

        Assert.NotNull(updatedMessage);
        Assert.Equal("Hello hello test updated", updatedMessage.Content);
    }

    private void AddMessagesToDb()
    {
        var message1 = new Message
        {
            SenderId = 1,
            ReceiverId = 2,
            Content = "Hello",
            SendAt = DateTime.UtcNow,
            IsRead = false
        };

        var message2 = new Message
        {
            SenderId = 2,
            ReceiverId = 1,
            Content = "Hi",
            SendAt = DateTime.UtcNow,
            IsRead = false
        };

        _dbContext.Messages.AddRange(message1, message2);
        _dbContext.SaveChanges();
    }
}
