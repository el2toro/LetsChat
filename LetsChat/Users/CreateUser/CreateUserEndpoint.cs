namespace LetsChat.Users.CreateUser;

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", async ([FromBody] UserDto userDto, ISender sender) =>
        {
            await sender.Send(new CreateUserRequest(userDto));
            return Results.Created();
        })
        .WithDisplayName("CreateUser")
        .Produces(StatusCodes.Status200OK)
        .WithDescription("Create User")
        .WithSummary("Create User");
    }
}
