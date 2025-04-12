namespace LetsChat.Messages.UpdateMessage;

public class UpdateMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/message", async ([FromBody] MessageDto messageDto, ISender sender) =>
        {
            var result = await sender.Send(new UpdateMessageRequest(messageDto));
            return Results.Ok(result.MessageDto);
        })
        .WithDisplayName("UpdateMessage");
    }
}
