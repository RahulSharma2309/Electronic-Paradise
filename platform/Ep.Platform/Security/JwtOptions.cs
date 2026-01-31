namespace Ep.Platform.Security
{
    /// <summary>
    /// Represents JWT configuration options.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Gets or sets the signing key used to generate and validate JWTs.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expected issuer.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expected audience.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether HTTPS metadata is required.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }
    }
}
