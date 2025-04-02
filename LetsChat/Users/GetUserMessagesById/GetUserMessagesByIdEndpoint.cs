namespace LetsChat.Users.GetUserMessagesById;

public class GetUserMessagesByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user", async ([FromQuery] int userId, int withUserId, ISender sender) =>
        {
            var result = await sender.Send(new GetUserMessagesByIdRequest(userId, withUserId));
            return result.Messages;
        });
    }
}
