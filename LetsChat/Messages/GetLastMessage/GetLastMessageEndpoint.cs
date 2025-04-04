namespace LetsChat.Messages.GetLastMessage;

public class GetLastMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/message", async ([FromQuery] int senderId, int receiverId, ISender sender) =>
        {
            var request = await sender.Send(new GetLastMessageRequest(senderId, receiverId));
            return Results.Ok(request.Message);
        })
        .WithName("GetLastMessage");
    }
}
