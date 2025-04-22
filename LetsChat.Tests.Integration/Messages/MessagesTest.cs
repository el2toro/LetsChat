using LetsChat.Data;
using LetsChat.Dtos;
using LetsChat.Messages.MarkMessageAsRead;
using LetsChat.Models;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LetsChat.Tests.Integration.Messages;

public partial class MessagesTest : IClassFixture<CustomWebAppFactoryIntegrationTest>
{
    private readonly HttpClient _client;
    private readonly LetsChatDbContext _context;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    public MessagesTest(CustomWebAppFactoryIntegrationTest factory)
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _client = factory.CreateClient();
        _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<LetsChatDbContext>();

        SeedDatabase();
    }

    [Fact]
    async Task SendMessage_Should_Return_StatusCode_201()
    {
        var request = new MessageDto
        {
            Content = "new message",
            SenderId = 3,
            ReceiverId = 4,
            SendAt = DateTime.UtcNow.ToString(),
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("message/", content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Theory]
    [InlineData("", 3, 4, "Message Content cannot be null or empty space")]
    [InlineData(null, 3, 4, "Message Content cannot be null or empty space")]
    [InlineData("some message", 0, 4, "SenderId should be greater than 0")]
    [InlineData("some message", 3, 0, "ReceiverId should be greater than 0")]
    async Task SendMessage_When_No_MessageContent_Throws_ValidationException
        (string? messageContent, int senderId, int receiverId, string expected)
    {
        var request = new MessageDto
        {
            Content = messageContent,
            SenderId = senderId,
            ReceiverId = receiverId,
            SendAt = DateTime.UtcNow.ToString(),
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("message/", content);
        var json = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(json, _jsonSerializerOptions);

        Assert.NotNull(errorResponse);
        Assert.Equal(nameof(ValidationException), errorResponse.Title);
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)errorResponse.Status);
        Assert.Contains(expected, errorResponse.Detail);
    }

    [Fact]
    async Task UpdateMessage_Shold_Return_Updated_Message()
    {
        var request = new MessageDto
        {
            Id = 1,
            Content = "updated message",
            SenderId = 1,
            ReceiverId = 2
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync("message/", content);
        var json = await response.Content.ReadAsStringAsync();
        var updatedMessage = JsonSerializer.Deserialize<MessageDto>(json, _jsonSerializerOptions);

        Assert.NotNull(updatedMessage);
        Assert.Equal("updated message", updatedMessage.Content);
        Assert.Equal(1, updatedMessage.SenderId);
        Assert.Equal(2, updatedMessage.ReceiverId);
    }

    [Theory]
    [InlineData("some message", 0, 1, 2, "Id should be greater than 0")]
    [InlineData("", 1,  1, 2, "Message Content cannot be null or empty space")]
    [InlineData(null, 1,  1, 2, "Message Content cannot be null or empty space")]
    [InlineData("some message", 1, 0, 4, "SenderId should be greater than 0")]
    [InlineData("some message", 1,  3, 0, "ReceiverId should be greater than 0")]
    async Task UpdateMessage__Return_Updated_Message
        (string? messageContent, int messageId,  int senderId, int receiverId, string expected)
    {
        var request = new MessageDto
        {
            Id = messageId,
            Content = messageContent,
            SenderId = senderId,
            ReceiverId = receiverId
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync("message/", content);
        var json = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(json, _jsonSerializerOptions);

        Assert.NotNull(errorResponse);
        Assert.Equal(nameof(ValidationException), errorResponse.Title);
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)errorResponse.Status);
        Assert.Contains(expected, errorResponse.Detail);
    }

    [Fact]
    async Task GetLastMessage_Shold_Return_Latest_Message()
    {
        string senderId = "1";
        string receiverId = "2";
        var url = $"message?senderId={Uri.EscapeDataString(senderId)}&receiverId={Uri.EscapeDataString(receiverId)}";

        var response = await _client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var lastMessage = JsonSerializer.Deserialize<MessageDto>(json, _jsonSerializerOptions);

        Assert.NotNull(lastMessage);
        Assert.Equal("latest message", lastMessage.Content);
    }

    [Fact]
    async Task GetMessages_Should_Return_Messages()
    {
        string senderId = "1";
        string receiverId = "2";
        var url = $"messages?senderId={Uri.EscapeDataString(senderId)}&receiverId={Uri.EscapeDataString(receiverId)}";

        var response = await _client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var messages = JsonSerializer.Deserialize<IEnumerable<Message>>(json, _jsonSerializerOptions);

        Assert.NotNull(messages);
        Assert.True(messages.Any());
    }

    [Fact]
    async Task MarkMessageAsRead_Should_Return_Messages_Marked_As_Read()
    {
        var request = new MarkMessagesAsReadRequest(1, 2);
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync("messages/", content);
        var json = await response.Content.ReadAsStringAsync();
        var messages = JsonSerializer.Deserialize<IEnumerable<Message>>(json, _jsonSerializerOptions);

        Assert.NotNull(messages);
        Assert.True(messages.Any());

        foreach (var message in messages)
        {
            Assert.True(message.IsRead);
        }
    }

    [Fact]
    async Task DeleteMessage_Should_Return_OK()
    {
        var response = await _client.DeleteAsync("message/3");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


    [Fact]
    async Task DeleteMessage_When_Message_Doesnt_Exists_Return_NotFound()
    {
        var response = await _client.DeleteAsync("message/156");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private void SeedDatabase()
    {
        var messages = new List<Message>
        {
            new Message
            {
                 Content = "first message",
                 SenderId = 1,
                 ReceiverId = 2,
                 SendAt = DateTime.UtcNow,
            },
            new Message
            {
                 Content = "another message",
                 SenderId = 1,
                 ReceiverId = 2,
                 SendAt = DateTime.UtcNow,
            },
            new Message
            {
                 Content = "latest message",
                 SenderId = 2,
                 ReceiverId = 1,
                 SendAt = DateTime.UtcNow,
            }
        };

        _context.Messages.AddRange(messages);
        _context.SaveChanges();
    }
}
