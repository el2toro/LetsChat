namespace LetsChat.Messages.SendMessage;

public record SendMessageResponse(bool IsSuccess);
public class SendMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/message", async ([FromBody] MessageDto message, ISender sender) =>
        {
            await sender.Send(new SendMesageRequest(message));
            //TODO: adapt result
            return Results.Created();
        })
        .WithName("SendMessage")
        .Produces<SendMessageResponse>()
        .WithDescription("Send Message")
        .WithSummary("Send Message");
    }
}
