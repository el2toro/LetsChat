using LetsChat.Auth.Dtos;

namespace LetsChat.Auth.Login;

public record LoginRequest(string UserName, string Password);
//public record LoginResponse(LoginDto LoginDto);
public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async ([FromBody] LoginRequest request, ISender sender) =>
        {
            var query = new LoginDto { UserName = request.UserName, Password = request.Password };
            var result = await sender.Send(new LoginQuery(query));

            return Results.Ok(result.LoginDto);
        })
        .WithName("Login");
    }
}
