using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.API;
using PaymentService.Core.Data;
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

namespace PaymentService.Integration.Test;

public class PaymentServiceFixture : IAsyncLifetime
{
    private readonly WebApplicationFactory<Startup> _factory;
    public HttpClient Client { get; }

    public PaymentServiceFixture()
    {
        _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ConnectionStrings:DefaultConnection", "Server=(localdb)\\MSSQLLocalDB;Database=PaymentServiceTestDb;Trusted_Connection=True;MultipleActiveResultSets=true" },
                    { "ServiceUrls:UserService", "http://user-service-mock:3001" }
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
                    options.UseInMemoryDatabase("InMemoryPaymentTestDb");
                });

                // Mock HttpClient for User Service wallet operations
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

                        if (request.Method == HttpMethod.Post && path.Contains("/wallet/debit", StringComparison.OrdinalIgnoreCase))
                        {
                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = JsonContent.Create(new { success = true }),
                            };
                        }

                        if (request.Method == HttpMethod.Post && path.Contains("/wallet/credit", StringComparison.OrdinalIgnoreCase))
                        {
                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = JsonContent.Create(new { success = true }),
                            };
                        }

                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json"),
                        };
                    });

                var httpClient = new HttpClient(handlerMock.Object)
                {
                    BaseAddress = new Uri("http://user-service-mock:3001"),
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



