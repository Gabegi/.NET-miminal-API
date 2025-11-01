namespace MinimalAPI.Extensions;

/// <summary>
/// Extension methods for HttpContext.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Gets the correlation ID from the current HTTP context.
    /// </summary>
    public static string GetCorrelationId(this HttpContext context)
    {
        return context.Items.TryGetValue("CorrelationId", out var correlationId)
            ? correlationId?.ToString() ?? context.TraceIdentifier
            : context.TraceIdentifier;
    }
}
