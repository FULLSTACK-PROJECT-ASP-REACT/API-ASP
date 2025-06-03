using System.Diagnostics;
using System.Net;
using System.Text.Json;
using ApiNetCore.Dtos;
using ApiNetCore.Exceptions;

namespace ApiNetCore.Controllers.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString();
        
        // Agregar el RequestId al contexto para usarlo en otros lugares
        context.Items["RequestId"] = requestId;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            await HandleExceptionAsync(context, ex, stopwatch.ElapsedMilliseconds, requestId);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, double responseTimeMs, string requestId)
    {
        _logger.LogError(exception, "An unhandled exception occurred. RequestId: {RequestId}", requestId);

        var response = context.Response;
        response.ContentType = "application/json";

        ApiResponse<object> apiResponse;

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = validationEx.StatusCode;
                apiResponse = ApiResponse<object>.ValidationErrorResponse(validationEx.Errors);
                break;

            case BaseException baseEx:
                response.StatusCode = baseEx.StatusCode;
                apiResponse = ApiResponse<object>.ErrorResponse(baseEx.Message, baseEx.ErrorCode);
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                apiResponse = ApiResponse<object>.ErrorResponse("Unauthorized access", "UNAUTHORIZED");
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                apiResponse = ApiResponse<object>.ErrorResponse(
                    "An internal server error occurred", 
                    "INTERNAL_SERVER_ERROR"
                );
                break;
        }

        apiResponse.ResponseTimeMs = responseTimeMs;
        apiResponse.RequestId = requestId;

        var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(jsonResponse);
    }
}