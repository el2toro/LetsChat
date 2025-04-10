namespace LetsChat.Users.UpdateUser;

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/user", async ([FromBody] UserDto userDto, ISender sender) =>
        {
            var result = await sender.Send(new UpdateUserRequest(userDto));
            return Results.Ok(result.UserDto);
        })
        .WithDisplayName("UpdateUser")
        .Produces(StatusCodes.Status200OK)
        .WithDescription("Update User")
        .WithSummary("Update User");
    }
}
