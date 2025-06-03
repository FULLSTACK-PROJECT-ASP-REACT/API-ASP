using System.Diagnostics;

namespace ApiNetCore.Controllers.Middleware;

public class ResponseTimeMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            context.Response.Headers.Add("X-Response-Time-ms", stopwatch.ElapsedMilliseconds.ToString());
            return Task.CompletedTask;
        });

        await _next(context);
    }
}