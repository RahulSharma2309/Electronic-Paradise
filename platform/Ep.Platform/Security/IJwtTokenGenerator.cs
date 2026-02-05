namespace Ep.Platform.Security
{
    /// <summary>
    /// Abstracts JWT token generation to avoid a direct JWT dependency in the business layer.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT token with custom claims.
        /// </summary>
        /// <param name="claims">The claims to include in the token.</param>
        /// <param name="expires">Optional token expiration (defaults to the platform default).</param>
        /// <returns>The serialized JWT.</returns>
        string GenerateToken(Dictionary<string, string> claims, TimeSpan? expires = null);
    }
}
