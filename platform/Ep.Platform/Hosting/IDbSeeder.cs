namespace Ep.Platform.Hosting
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines a database seeder for a specific DbContext type.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    public interface IDbSeeder<in TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Seeds the database.
        /// </summary>
        /// <param name="db">The DbContext instance.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous seeding operation.</returns>
        Task SeedAsync(TContext db, CancellationToken cancellationToken = default);
    }
}
