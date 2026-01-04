using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ep.Platform.Hosting;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext db, CancellationToken cancellationToken = default);
}

public static class HostDatabaseExtensions
{
    /// <summary>
    /// Applies migrations (or creates the database) and runs any registered seeders.
    /// Intended to replace the ad-hoc EnsureCreated calls in Program.cs.
    /// </summary>
    public static async Task EnsureDatabaseAsync<TContext>(
        this IHost host,
        bool applyMigrations = true,
        CancellationToken cancellationToken = default)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TContext>();

        if (applyMigrations)
        {
            await db.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            await db.Database.EnsureCreatedAsync(cancellationToken);
        }

        var seeders = scope.ServiceProvider.GetServices<IDbSeeder<TContext>>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(db, cancellationToken);
        }
    }
}















