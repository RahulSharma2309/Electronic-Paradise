namespace Ep.Platform.Hosting
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Provides host extensions for ensuring the database is created/migrated and seeded.
    /// </summary>
    public static class HostDatabaseExtensions
    {
        /// <summary>
        /// Applies migrations (or creates the database) and runs any registered seeders.
        /// Intended to replace ad-hoc database initialization calls in Program.cs.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type.</typeparam>
        /// <param name="host">The host instance.</param>
        /// <param name="applyMigrations">Whether to apply EF Core migrations.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        public static async Task EnsureDatabaseAsync<TContext>(
            this IHost host,
            bool applyMigrations = true,
            CancellationToken cancellationToken = default)
            where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TContext>();

            // Migrations are only supported for relational providers.
            // For in-memory/testing providers, fall back to EnsureCreated.
            if (applyMigrations && db.Database.IsRelational())
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
}
