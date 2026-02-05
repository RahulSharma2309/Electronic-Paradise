using Ep.Platform.DependencyInjection;
using Ep.Platform.Hosting;
using UserService.Core.Business;
using UserService.Core.Data;
using UserService.Core.Mappers;
using UserService.Core.Repository;

namespace UserService.API;

/// <summary>
/// Application startup configuration for the User service.
/// </summary>
public class Startup
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="config">The application configuration.</param>
    public Startup(IConfiguration config) => _config = config;

    /// <summary>
    /// Configures dependency injection services for the User service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Use Platform extensions for infrastructure
        services.AddEpSwaggerWithJwt("User Service", "v1");
        services.AddEpDefaultCors("AllowLocalhost3000", new[] { "http://localhost:3000" });
        services.AddEpSqlServerDbContext<AppDbContext>(_config);

        // Register business and repository services
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserServiceImpl>();

        // Register mapper
        services.AddScoped<IUserProfileMapper, UserProfileMapper>();
    }

    /// <summary>
    /// Configures the HTTP request pipeline for the User service.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The host environment.</param>
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors("AllowLocalhost3000");
        app.UseAuthorization();
        app.MapControllers();
    }
}
