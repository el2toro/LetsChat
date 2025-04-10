using LetsChat.Users.GetUsers;

namespace LetsChat.Users.DeleteUser;

public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user/{userId}", async (int userId, ISender sender) =>
        {
            await sender.Send(new DeleteUserRequest(userId));
            return Results.NoContent();
        })
        .WithDisplayName("DeleteUser")
        .Produces<GetUsersResult>(StatusCodes.Status200OK)
        .WithDescription("Delete User")
        .WithSummary("Delete User");
    }
}
