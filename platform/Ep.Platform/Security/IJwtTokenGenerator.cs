namespace Ep.Platform.Security;

/// <summary>
/// Abstracts JWT token generation to avoid direct JWT dependency in business layer
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generate a JWT token with custom claims
    /// </summary>
    string GenerateToken(Dictionary<string, string> claims, TimeSpan? expires = null);
}

