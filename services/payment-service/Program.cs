using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

var builder = WebApplication.CreateBuilder(args);

var startup = new PaymentService.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Ensure DB created and seed sample wallet for local dev
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Wallets.Any())
    {
        db.Wallets.Add(new Wallet { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Balance = 5000 });
        db.SaveChanges();
    }
}

startup.Configure(app, app.Environment as Microsoft.AspNetCore.Hosting.IWebHostEnvironment ?? throw new InvalidOperationException("Missing environment"));

app.Run();


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<PaymentRecord> Payments { get; set; }
}

public class Wallet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    // Balance stored in cents
    public int Balance { get; set; }
}
