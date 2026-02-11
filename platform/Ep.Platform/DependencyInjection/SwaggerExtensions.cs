namespace Ep.Platform.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// Provides DI extension methods for registering Swagger/OpenAPI.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger with a standard JWT bearer security definition used by the services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="title">The API title displayed in Swagger UI.</param>
        /// <param name="version">The API version used by Swagger (defaults to <c>v1</c>).</param>
        /// <returns>The same service collection to allow call chaining.</returns>
        public static IServiceCollection AddEpSwaggerWithJwt(
            this IServiceCollection services,
            string title,
            string version = "v1")
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                });
            });

            return services;
        }
    }
}
