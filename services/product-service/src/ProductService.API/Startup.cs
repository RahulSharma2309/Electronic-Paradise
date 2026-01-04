using Ep.Platform.DependencyInjection;
using Ep.Platform.Hosting;
using ProductService.Core.Business;
using ProductService.Core.Data;
using ProductService.Core.Repository;

namespace ProductService.API;

/// <summary>
/// Application startup configuration for the Product service.
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
    /// Configures dependency injection services for the Product service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Platform extensions
        services.AddEpSwaggerWithJwt("Product Service", "v1");
        services.AddEpDefaultCors("AllowLocalhost3000", new[] { "http://localhost:3000" });
        services.AddEpSqlServerDbContext<AppDbContext>(_config);

        // Register business and repository
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductServiceImpl>();
    }

    /// <summary>
    /// Configures the HTTP request pipeline for the Product service.
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
