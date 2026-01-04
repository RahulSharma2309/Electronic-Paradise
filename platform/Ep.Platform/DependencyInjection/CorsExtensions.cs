using Microsoft.Extensions.DependencyInjection;

namespace Ep.Platform.DependencyInjection;

public static class CorsExtensions
{
    /// <summary>
    /// Adds a simple CORS policy intended for SPA localhost development.
    /// </summary>
    public static IServiceCollection AddEpDefaultCors(
        this IServiceCollection services,
        string policyName = "AllowLocalhost3000",
        string[]? origins = null)
    {
        origins ??= new[]
        {
            "http://localhost:3000",
            "http://localhost:4200",
        };

        services.AddCors(options =>
        {
            options.AddPolicy(policyName, builder =>
            {
                builder.WithOrigins(origins)
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });

        return services;
    }
}















