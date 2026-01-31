using Ep.Platform.DependencyInjection;
using Ep.Platform.Hosting;
using PaymentService.Core.Business;
using PaymentService.Core.Data;
using PaymentService.Core.Mappers;
using PaymentService.Core.Repository;

namespace PaymentService.API;

/// <summary>
/// Application startup configuration for the Payment service.
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
    /// Configures dependency injection services for the Payment service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Platform extensions for infrastructure
        services.AddEpSwaggerWithJwt("Payment Service", "v1");
        services.AddEpDefaultCors("AllowLocalhost3000", new[] { "http://localhost:3000" });
        services.AddEpSqlServerDbContext<AppDbContext>(_config);

        // HttpClient for User Service
        services.AddEpHttpClient("user", _config, "ServiceUrls:UserService");

        // Register business and repository services
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentService, PaymentServiceImpl>();

        // Register mapper
        services.AddScoped<IPaymentMapper, PaymentMapper>();
    }

    /// <summary>
    /// Configures the HTTP request pipeline for the Payment service.
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
