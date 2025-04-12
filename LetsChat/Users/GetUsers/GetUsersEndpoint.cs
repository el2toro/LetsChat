namespace LetsChat.Users.GetUsers;

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (ISender sender) =>
        {
            var result = await sender.Send(new GetUsersRequest());
            return Results.Ok(result.Users);
        })
        .WithDisplayName("GetUsers")
        .Produces(StatusCodes.Status200OK)
        .WithDescription("Get Users")
        .WithSummary("Get Users");
    }
}
