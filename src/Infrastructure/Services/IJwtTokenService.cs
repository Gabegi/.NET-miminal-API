using Infrastructure.Entities;

namespace Infrastructure.Services;

/// <summary>
/// Service for generating and validating JWT tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    string GenerateToken(User user);
}
