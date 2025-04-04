namespace LetsChat.Messages.MarkMessageAsRead;

public record MarkMessagesAsReadRequest(int SenderId, int ReceiverId);

public class MarkMessagesAsReadEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/messages", async ([FromBody] MarkMessagesAsReadRequest request, ISender sender) =>
        {
            await sender.Send(new MarkMessagesAsReadQuery(request.SenderId, request.ReceiverId));
            return Results.Ok();
        })
        .WithName("MarkMessagesAsRead");
    }
}
