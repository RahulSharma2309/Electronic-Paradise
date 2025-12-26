using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

var builder = WebApplication.CreateBuilder(args);

// Delegate service registration to Startup
var startup = new ProductService.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Ensure DB created and seed sample products for local dev
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.AddRange(new Product { Name = "T-Shirt", Description = "Plain white T-Shirt", Price = 1999, Stock = 10 },
                             new Product { Name = "Coffee Mug", Description = "Ceramic mug", Price = 999, Stock = 5 },
                             new Product { Name = "Notebook", Description = "A5 ruled notebook", Price = 499, Stock = 0 });
        db.SaveChanges();
    }
}

startup.Configure(app, app.Environment as Microsoft.AspNetCore.Hosting.IWebHostEnvironment ?? throw new InvalidOperationException("Missing environment"));

app.Run();
