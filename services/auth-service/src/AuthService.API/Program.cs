using AuthService.API;
using AuthService.Core.Data;
using Ep.Platform.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Use Startup class for configuration
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Ensure database is created (for development)
await app.EnsureDatabaseAsync<AppDbContext>(applyMigrations: false);

// Configure middleware pipeline
startup.Configure(app, app.Environment);

app.Run();
