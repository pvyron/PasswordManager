using System.Globalization;

namespace PasswordManager.Api.Middlewares;

public sealed class ResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    
        var response = context.Response;
    }
}
