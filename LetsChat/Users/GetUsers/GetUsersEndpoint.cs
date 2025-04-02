namespace LetsChat.Users.GetUsers;

public record GetUsersResult(IEnumerable<User> Users);

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (ISender sender) =>
        {
            var result = await sender.Send(new GetUserRequest());
            return Results.Ok(result.Users);
        })
        .WithDisplayName("GetUsers")
        .Produces<GetUsersResult>(StatusCodes.Status200OK)
        .WithDescription("Get Users")
        .WithSummary("Get Users");
    }
}
