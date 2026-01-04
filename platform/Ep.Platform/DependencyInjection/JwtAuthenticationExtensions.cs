using System.Text;
using Ep.Platform.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Ep.Platform.DependencyInjection;

public static class JwtAuthenticationExtensions
{
    /// <summary>
    /// Configures JWT bearer authentication using the "Jwt" section by default.
    /// </summary>
    public static IServiceCollection AddEpJwtAuth(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "Jwt")
    {
        var section = configuration.GetSection(sectionName);
        var options = section.Get<JwtOptions>() ?? new JwtOptions();

        if (string.IsNullOrWhiteSpace(options.Key))
            throw new InvalidOperationException($"Jwt configuration section '{sectionName}' must include a non-empty Key.");
        if (string.IsNullOrWhiteSpace(options.Issuer))
            throw new InvalidOperationException($"Jwt configuration section '{sectionName}' must include a non-empty Issuer.");
        if (string.IsNullOrWhiteSpace(options.Audience))
            throw new InvalidOperationException($"Jwt configuration section '{sectionName}' must include a non-empty Audience.");

        services.Configure<JwtOptions>(section);

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));

        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, bearerOptions =>
        {
            bearerOptions.RequireHttpsMetadata = options.RequireHttpsMetadata;
            bearerOptions.SaveToken = true;
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                IssuerSigningKey = signingKey
            };
        });

        return services;
    }
}
















