namespace LetsChat.Messages.MarkMessageAsRead;

public record MarkMessagesAsReadRequest(int SenderId, int ReceiverId);

public class MarkMessagesAsReadEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/messages", async ([FromBody] MarkMessagesAsReadRequest request, ISender sender) =>
        {
            var result = await sender.Send(new MarkMessagesAsReadQuery(request.SenderId, request.ReceiverId));
            return Results.Ok(result.Messages);
        })
        .WithName("MarkMessagesAsRead");
    }
}
