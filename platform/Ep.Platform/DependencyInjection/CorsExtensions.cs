namespace Ep.Platform.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides DI extension methods for adding CORS configuration.
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Adds a simple CORS policy intended for SPA localhost development.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="policyName">The name of the CORS policy to register.</param>
        /// <param name="origins">Optional allowed origins (defaults to common localhost SPA ports).</param>
        /// <returns>The same service collection to allow call chaining.</returns>
        public static IServiceCollection AddEpDefaultCors(
            this IServiceCollection services,
            string policyName = "AllowLocalhost3000",
            string[]? origins = null)
        {
            origins ??=
            [
                "http://localhost:3000",
                "http://localhost:4200",
            ];

            services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                {
                    builder
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
