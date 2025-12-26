using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Determine which config file to load
var useDocker = builder.Configuration["USE_DOCKER_NETWORK"] ?? Environment.GetEnvironmentVariable("USE_DOCKER_NETWORK");
var configFile = string.Equals(useDocker, "true", StringComparison.OrdinalIgnoreCase) ? "ocelot.docker.json" : "ocelot.json";

// Load the config file into the configuration
builder.Configuration.AddJsonFile(configFile, optional: false, reloadOnChange: true);

// Delegate to Startup to configure services and middleware
var startup = new Gateway.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment as Microsoft.AspNetCore.Hosting.IWebHostEnvironment ?? throw new InvalidOperationException("Missing environment"));

app.MapReverseProxy();

app.Run();
