using Ep.Platform.DependencyInjection;
using Ep.Platform.Hosting;
using ProductService.Core.Business;
using ProductService.Core.Data;
using ProductService.Core.Repository;

namespace ProductService.API;

public class Startup
{
    private readonly IConfiguration _config;
    public Startup(IConfiguration config) => _config = config;

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