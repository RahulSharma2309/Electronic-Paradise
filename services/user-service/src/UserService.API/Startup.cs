using Ep.Platform.DependencyInjection;
using Ep.Platform.Hosting;
using UserService.Core.Business;
using UserService.Core.Data;
using UserService.Core.Repository;

namespace UserService.API;

public class Startup
{
    private readonly IConfiguration _config;
    public Startup(IConfiguration config) => _config = config;

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




