using Microsoft.AspNetCore.SignalR;

namespace LetsChat.Hubs;

public class ChatHub : Hub
{
    // Store online users (can use a database or cache for persistence)
    private static readonly HashSet<string> OnlineUsers = new HashSet<string>();
    public async Task SendMessage(MessageDto message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    // Triggered when a user connects to the SignalR hub
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier; // This could be the user ID or any identifier
        OnlineUsers.Add(userId);

        // Notify other users that the user is online (you can send this info to all clients)
        await Clients.All.SendAsync("UserOnline", userId);

        await base.OnConnectedAsync();
    }

    // Triggered when a user disconnects
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;
        OnlineUsers.Remove(userId);

        // Notify other users that the user has gone offline
        await Clients.All.SendAsync("UserOffline", userId);

        await base.OnDisconnectedAsync(exception);
    }

    // Get the list of online users (optional)
    public Task<IEnumerable<string>> GetOnlineUsers()
    {
        return Task.FromResult(OnlineUsers.AsEnumerable());
    }
}
