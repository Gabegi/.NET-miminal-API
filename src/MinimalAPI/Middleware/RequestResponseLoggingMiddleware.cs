using System.Diagnostics;

namespace MinimalAPI.Middleware;

/// <summary>
/// Middleware for logging HTTP requests and responses with performance metrics.
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? context.TraceIdentifier;

        // Log incoming request
        _logger.LogInformation(
            "HTTP REQUEST: {Method} {Path} | CorrelationId: {CorrelationId} | Scheme: {Scheme}",
            context.Request.Method,
            context.Request.Path,
            correlationId,
            context.Request.Scheme);

        // Store original response stream
        var originalBodyStream = context.Response.Body;

        // Create a memory stream to capture response body
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                stopwatch.Stop();

                // Log successful response
                _logger.LogInformation(
                    "HTTP RESPONSE: {Method} {Path} | StatusCode: {StatusCode} | Duration: {ElapsedMilliseconds}ms | CorrelationId: {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds,
                    correlationId);

                // Copy response body to original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex,
                    "HTTP ERROR: {Method} {Path} | StatusCode: {StatusCode} | Duration: {ElapsedMilliseconds}ms | CorrelationId: {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds,
                    correlationId);

                throw;
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }
}
