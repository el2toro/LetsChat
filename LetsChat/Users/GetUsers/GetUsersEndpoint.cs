namespace LetsChat.Users.GetUsers;

public record GetUsersResult(IEnumerable<UserDto> Users);

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async ([FromQuery] int senderId, ISender sender) =>
        {
            var result = await sender.Send(new GetUserRequest(senderId));
            return Results.Ok(result.Users);
        })
        .WithDisplayName("GetUsers")
        .Produces<GetUsersResult>(StatusCodes.Status200OK)
        .WithDescription("Get Users")
        .WithSummary("Get Users");
    }
}
