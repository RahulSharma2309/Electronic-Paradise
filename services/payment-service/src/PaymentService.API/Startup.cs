using Ep.Platform.DependencyInjection;
using Ep.Platform.Hosting;
using PaymentService.Core.Business;
using PaymentService.Core.Data;
using PaymentService.Core.Repository;

namespace PaymentService.API;

public class Startup
{
    private readonly IConfiguration _config;
    public Startup(IConfiguration config) => _config = config;

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
    }

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




