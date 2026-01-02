using AuthService.Core.Business;
using AuthService.Core.Data;
using AuthService.Core.Repository;
using Ep.Platform.DependencyInjection;
using Ep.Platform.Hosting;

namespace AuthService.API;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configure services for dependency injection
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        // Add controllers
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Use Ep.Platform extensions
        services.AddEpSwaggerWithJwt("Auth Service", "v1");
        services.AddEpDefaultCors("AllowLocalhost3000", new[] { "http://localhost:3000" });

        // Database (Platform handles EF Core)
        services.AddEpSqlServerDbContext<AppDbContext>(Configuration);

        // HttpClient for User Service (Platform handles HttpClient + Polly)
        services.AddEpHttpClient("user", Configuration, "ServiceUrls:UserService");

        // JWT Authentication (Platform handles JWT Bearer configuration)
        services.AddEpJwtAuth(Configuration);

        // Security Services (Platform provides password hashing and JWT token generation)
        services.AddEpSecurityServices();

        // Register business and repository services (Auth Service specific)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, Core.Business.AuthService>();
    }

    /// <summary>
    /// Configure the HTTP request pipeline
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors("AllowLocalhost3000");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

