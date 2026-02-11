namespace Ep.Platform.DependencyInjection
{
    using Ep.Platform.Security;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides DI extension methods for registering platform security services.
    /// </summary>
    public static class SecurityExtensions
    {
        /// <summary>
        /// Registers platform security services (password hashing and JWT token generation).
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection to allow call chaining.</returns>
        public static IServiceCollection AddEpSecurityServices(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
