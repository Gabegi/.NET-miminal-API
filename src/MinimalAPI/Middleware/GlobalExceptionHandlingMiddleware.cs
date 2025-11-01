using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Infrastructure.Exceptions;
using AppException = Infrastructure.Exceptions.ApplicationException;
using AppUnauthorizedAccessException = Infrastructure.Exceptions.UnauthorizedAccessException;

namespace MinimalAPI.Middleware;

/// <summary>
/// Global exception handling middleware that catches all unhandled exceptions.
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Get or create correlation ID
        var correlationId = context.Request.Headers.ContainsKey("X-Correlation-Id")
            ? context.Request.Headers["X-Correlation-Id"].ToString()
            : context.TraceIdentifier;

        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred. CorrelationId: {CorrelationId}", correlationId);
            await HandleExceptionAsync(context, ex, correlationId);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            CorrelationId = correlationId,
            Timestamp = DateTime.UtcNow,
            TraceId = Activity.Current?.Id ?? context.TraceIdentifier
        };

        switch (exception)
        {
            case EntityNotFoundException ex:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Title = "Not Found";
                response.Detail = ex.Message;
                response.Code = ex.Code;
                break;

            case ValidationException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Title = "Validation Error";
                response.Detail = ex.Message;
                response.Code = ex.Code;
                response.Errors = ex.Errors.Any()
                    ? ex.Errors.ToDictionary(x => x.Key, x => x.Value)
                    : null;
                break;

            case AppUnauthorizedAccessException ex:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                response.StatusCode = StatusCodes.Status403Forbidden;
                response.Title = "Forbidden";
                response.Detail = ex.Message;
                response.Code = ex.Code;
                break;

            case AppException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Title = "Bad Request";
                response.Detail = ex.Message;
                response.Code = ex.Code;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Title = "Internal Server Error";
                response.Detail = "An unexpected error occurred. Please try again later.";
                response.Code = "INTERNAL_SERVER_ERROR";
#if !DEBUG
                // Don't expose internal details in production
                response.Exception = null;
#else
                response.Exception = new ExceptionDetail
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    Type = exception.GetType().Name
                };
#endif
                break;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsJsonAsync(response, options);
    }
}

/// <summary>
/// Standard error response structure (ProblemDetails-like).
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string CorrelationId { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public IDictionary<string, string[]>? Errors { get; set; }
    public ExceptionDetail? Exception { get; set; }
}

/// <summary>
/// Exception details (shown in debug mode only).
/// </summary>
public class ExceptionDetail
{
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string Type { get; set; } = string.Empty;
}
