using Ep.Platform.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Ep.Platform.DependencyInjection;

public static class SecurityExtensions
{
    /// <summary>
    /// Registers platform security services (Password Hashing and JWT Token Generation)
    /// </summary>
    public static IServiceCollection AddEpSecurityServices(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        
        return services;
    }
}














