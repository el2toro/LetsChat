namespace LetsChat.Messages.GetMessages;

public class GetMessagesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/messages", async ([FromQuery] int senderId, int receiverId, ISender sender) =>
        {
            var result = await sender.Send(new GetMessagesRequest(senderId, receiverId));
            return Results.Ok(result.Messages);
        })
        .WithName("GetMessages")
        .RequireRateLimiting("fixed");
    }
}
