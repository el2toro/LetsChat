namespace LetsChat.Users.GetUserById;

public class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/{userId}", async (int userId, ISender sender) =>
        {
            var result = await sender.Send(new GetUserByIdRequest(userId));
            return Results.Ok(result.UserDto);
        })
        .WithDisplayName("GetUserById")
        .Produces(StatusCodes.Status200OK)
        .WithDescription("Get User By Id")
        .WithSummary("Get User By Id");
    }
}
