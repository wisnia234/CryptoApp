namespace CryptoApp.Server.Middleware;
using CryptoApp.Server.Exceptions;

internal sealed class ExceptionMiddleware : IMiddleware
{

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        try
        {
            await next(context);
        }
        catch (Exception ex)
        { 
            await HandleExceptionAsync(context, ex);
        }

    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error) = exception switch
        {
            BaseException => (StatusCodes.Status400BadRequest, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "There was an internal error.")
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }
}
