using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BooksRUs.Host.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ctx, ex, _logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext ctx, Exception ex, ILogger logger)
    {
        var (status, title, detail) = ex switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, "Invalid request", ex.Message),
            InvalidOperationException => (HttpStatusCode.Conflict, "Conflict", ex.Message),
            _ => (HttpStatusCode.InternalServerError, "Server error", "Unexpected error occurred.")
        };

        // Log once, structured
        logger.LogError(ex, "Unhandled exception. Status={Status} Path={Path}", (int)status, ctx.Request.Path);

        // RFC7807 ProblemDetails response
        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = detail,
            Instance = ctx.Request.Path
        };
        problem.Extensions["traceId"] = ctx.TraceIdentifier;

        ctx.Response.Clear();
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = (int)status;
        await ctx.Response.WriteAsJsonAsync(problem);
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ErrorHandlingMiddleware>();
}
