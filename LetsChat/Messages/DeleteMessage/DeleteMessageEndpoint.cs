namespace LetsChat.Messages.DeleteMessage;

public class DeleteMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/message/{messageId}", async (int messageId, ISender sender) =>
        {
            await sender.Send(new DeleteMessageRequest(messageId));
            return Results.NoContent();
        })
        .WithDisplayName("DeleteMessage");
    }
}
