using Ep.Platform.DependencyInjection;
using Ep.Platform.Hosting;
using OrderService.Core.Business;
using OrderService.Core.Data;
using OrderService.Core.Repository;

namespace OrderService.API;

/// <summary>
/// Application startup configuration for the Order service.
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
    /// Configures dependency injection services for the Order service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Platform extensions
        services.AddEpSwaggerWithJwt("Order Service", "v1");
        services.AddEpDefaultCors("AllowLocalhost3000", new[] { "http://localhost:3000" });
        services.AddEpSqlServerDbContext<AppDbContext>(_config);

        // HttpClients for service-to-service communication
        services.AddEpHttpClient("user", _config, "ServiceUrls:UserService");
        services.AddEpHttpClient("product", _config, "ServiceUrls:ProductService");
        services.AddEpHttpClient("payment", _config, "ServiceUrls:PaymentService");

        // Register business and repository
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderServiceImpl>();
    }

    /// <summary>
    /// Configures the HTTP request pipeline for the Order service.
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
