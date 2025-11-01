namespace Infrastructure.Configuration;

/// <summary>
/// JWT configuration settings.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    /// <summary>
    /// Secret key for signing JWT tokens (store in user-secrets).
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// JWT token issuer.
    /// </summary>
    public string Issuer { get; set; } = "MinimalAPI";

    /// <summary>
    /// JWT token audience.
    /// </summary>
    public string Audience { get; set; } = "MinimalAPIClients";

    /// <summary>
    /// Token expiry in minutes.
    /// </summary>
    public int ExpiryMinutes { get; set; } = 60;
}
