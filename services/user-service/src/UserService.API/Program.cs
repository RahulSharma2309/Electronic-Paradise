using UserService.API;
using UserService.Core.Data;
using Ep.Platform.Hosting;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Ensure database is created (for development)
await app.EnsureDatabaseAsync<AppDbContext>(applyMigrations: false);

startup.Configure(app, app.Environment);

app.Run();
