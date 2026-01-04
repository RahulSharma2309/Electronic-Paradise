using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.API;
using ProductService.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;

namespace ProductService.Integration.Test;

public class ProductServiceFixture : IAsyncLifetime
{
    private readonly WebApplicationFactory<Startup> _factory;
    public HttpClient Client { get; }

    public ProductServiceFixture()
    {
        _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:DefaultConnection", "Server=(localdb)\\MSSQLLocalDB;Database=ProductServiceTestDb;Trusted_Connection=True;MultipleActiveResultSets=true" }
                });
            });

            builder.ConfigureServices(services =>
            {
                // Replace the real DbContext with an in-memory one for isolated testing
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryProductTestDb");
                });

                // Ensure the database is created for the in-memory context
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                }
            });
        });

        Client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        // Any setup that needs to happen before tests run
        var response = await Client.GetAsync("/api/health");
        response.EnsureSuccessStatusCode();
    }

    public async Task DisposeAsync()
    {
        // Clean up resources after all tests in the collection have run
        await _factory.DisposeAsync();
    }
}



