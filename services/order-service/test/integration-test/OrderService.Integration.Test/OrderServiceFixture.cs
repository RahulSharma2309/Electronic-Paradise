using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.API;
using OrderService.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using System.Threading;
using OrderService.Abstraction.DTOs;

namespace OrderService.Integration.Test;

public class OrderServiceFixture : IAsyncLifetime
{
    private readonly WebApplicationFactory<Startup> _factory;
    public HttpClient Client { get; }

    public OrderServiceFixture()
    {
        _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ConnectionStrings:DefaultConnection", "Server=(localdb)\\MSSQLLocalDB;Database=OrderServiceTestDb;Trusted_Connection=True;MultipleActiveResultSets=true" },
                    { "ServiceUrls:UserService", "http://dependency-mock:3001" },
                    { "ServiceUrls:ProductService", "http://dependency-mock:3001" },
                    { "ServiceUrls:PaymentService", "http://dependency-mock:3001" }
                });
            });

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryOrderTestDb");
                });

                // Mock HttpClient for downstream service calls (user/product/payment)
                var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
                handlerMock
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                    {
                        var path = request.RequestUri?.AbsolutePath ?? string.Empty;

                        // User service: return a valid user profile
                        if (request.Method == HttpMethod.Get && path.StartsWith("/api/users/by-userid/", StringComparison.OrdinalIgnoreCase))
                        {
                            var userIdStr = path["/api/users/by-userid/".Length..];
                            var userId = Guid.TryParse(userIdStr, out var guid) ? guid : Guid.NewGuid();

                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = JsonContent.Create(new UserProfileDto
                                {
                                    Id = Guid.NewGuid(),
                                    UserId = userId,
                                    FirstName = "Integration",
                                    LastName = "User",
                                    WalletBalance = 1_000_000,
                                    PhoneNumber = "+10000000000",
                                }),
                            };
                        }

                        // Product service: return a product with ample stock and a fixed price
                        if (request.Method == HttpMethod.Get && path.StartsWith("/api/products/", StringComparison.OrdinalIgnoreCase))
                        {
                            var productIdStr = path["/api/products/".Length..];
                            var productId = Guid.TryParse(productIdStr, out var guid) ? guid : Guid.NewGuid();

                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = JsonContent.Create(new ProductDto
                                {
                                    Id = productId,
                                    Name = "Mock Product",
                                    Price = 10,
                                    Stock = 9999,
                                }),
                            };
                        }

                        // Product service: reserve/release endpoints - always succeed
                        if (request.Method == HttpMethod.Post && path.Contains("/api/products/", StringComparison.OrdinalIgnoreCase) &&
                            (path.EndsWith("/reserve", StringComparison.OrdinalIgnoreCase) || path.EndsWith("/release", StringComparison.OrdinalIgnoreCase)))
                        {
                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = JsonContent.Create(new { remaining = 9999 }),
                            };
                        }

                        // Payment service: process endpoint - always succeed
                        if (request.Method == HttpMethod.Post && path.Equals("/api/payments/process", StringComparison.OrdinalIgnoreCase))
                        {
                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = JsonContent.Create(new { success = true }),
                            };
                        }

                        // Payment service: refund endpoint - always succeed
                        if (request.Method == HttpMethod.Post && path.Equals("/api/payments/refund", StringComparison.OrdinalIgnoreCase))
                        {
                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = JsonContent.Create(new { success = true }),
                            };
                        }

                        // Default fallback
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json"),
                        };
                    });

                var httpClient = new HttpClient(handlerMock.Object)
                {
                    BaseAddress = new Uri("http://dependency-mock:3001"),
                };

                var mockFactory = new Mock<IHttpClientFactory>();
                mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
                services.AddSingleton(mockFactory.Object);

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
        var response = await Client.GetAsync("/api/health");
        response.EnsureSuccessStatusCode();
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}



