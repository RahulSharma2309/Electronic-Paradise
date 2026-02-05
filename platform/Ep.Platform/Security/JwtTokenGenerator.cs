namespace Ep.Platform.Security
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// JWT token generator implementation.
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
        /// </summary>
        /// <param name="options">The JWT options.</param>
        public JwtTokenGenerator(IOptions<JwtOptions> options)
        {
            this.options = options.Value;
        }

        /// <inheritdoc />
        public string GenerateToken(Dictionary<string, string> claims, TimeSpan? expires = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(this.options.Key);

            var claimsList = claims.Select(kvp => new Claim(kvp.Key, kvp.Value)).ToArray();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimsList),
                Expires = DateTime.UtcNow.Add(expires ?? TimeSpan.FromHours(6)),
                Issuer = this.options.Issuer,
                Audience = this.options.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
