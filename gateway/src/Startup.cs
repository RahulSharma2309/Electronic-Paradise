using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Gateway;

public class Startup
{
    private readonly IConfiguration _config;
    public Startup(IConfiguration config) => _config = config;

    public void ConfigureServices(IServiceCollection services)
    {
        var useDocker = _config.GetValue<string>("USE_DOCKER_NETWORK") ?? Environment.GetEnvironmentVariable("USE_DOCKER_NETWORK");
        var configFile = string.Equals(useDocker, "true", StringComparison.OrdinalIgnoreCase) ? "ocelot.docker.json" : "ocelot.json";
        // Load the chosen config file so YARP can pick it up from config
        services.Configure<Microsoft.Extensions.Configuration.IConfiguration>(c => { });

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" }));

    services.AddReverseProxy().LoadFromConfig(_config.GetSection("ReverseProxy"));
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

    // No authorization in gateway for now; gateway maps the reverse proxy routes
    app.MapControllers();
    }
}
