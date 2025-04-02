namespace LetsChat.Users.CreateUser;

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", (User user, ISender sender) =>
        {

        })
        .WithName("GetUser");
    }
}
