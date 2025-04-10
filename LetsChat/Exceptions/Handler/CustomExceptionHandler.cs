using Microsoft.AspNetCore.Diagnostics;

namespace LetsChat.Exceptions.Handler;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        (string Details, string Title, int StatusCode) details = exception switch
        {
            InternalServerException =>
            (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
            //ValidationException =>
            //(
            //   exception.Message,
            //   exception.GetType().Name,
            //   context.Response.StatusCode = StatusCodes.Status400BadRequest
            //),
            BadRequestException =>
            (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            NotFoundException =>
            (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            _ =>
            (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Details,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
