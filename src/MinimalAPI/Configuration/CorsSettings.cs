namespace MinimalAPI.Configuration;

/// <summary>
/// CORS configuration settings.
/// </summary>
public class CorsSettings
{
    public const string SectionName = "Cors";

    /// <summary>
    /// CORS policy name.
    /// </summary>
    public string PolicyName { get; set; } = "AllowSpecificOrigins";

    /// <summary>
    /// Allowed origins for CORS requests.
    /// </summary>
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Allowed HTTP methods.
    /// </summary>
    public string[] AllowedMethods { get; set; } = ["GET", "POST", "PUT", "DELETE", "OPTIONS"];

    /// <summary>
    /// Allowed headers.
    /// </summary>
    public string[] AllowedHeaders { get; set; } = ["*"];

    /// <summary>
    /// Exposed headers.
    /// </summary>
    public string[] ExposedHeaders { get; set; } = ["*"];

    /// <summary>
    /// Allow credentials in CORS requests.
    /// </summary>
    public bool AllowCredentials { get; set; } = true;

    /// <summary>
    /// Max age in seconds for preflight requests.
    /// </summary>
    public int MaxAge { get; set; } = 3600;
}
