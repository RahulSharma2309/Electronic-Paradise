using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthService.API;
using AuthService.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Net.Http.Json;
using AuthService.Abstraction.DTOs;
using Moq;
using Moq.Protected;
using System.Threading;
using System.Net;

namespace AuthService.Integration.Test;

public class AuthServiceFixture : IAsyncLifetime
{
    private readonly WebApplicationFactory<Startup> _factory;
    public HttpClient Client { get; }

    public AuthServiceFixture()
    {
        _factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:DefaultConnection", "Server=(localdb)\\MSSQLLocalDB;Database=AuthServiceTestDb;Trusted_Connection=True;MultipleActiveResultSets=true" },
                    { "ServiceUrls:UserService", "http://user-service-mock:3001" },
                    { "Jwt:Key", "your-super-secret-key-that-should-be-at-least-32-characters-long-for-security" },
                    { "Jwt:Issuer", "Electronic-Paradise" },
                    { "Jwt:Audience", "Electronic-Paradise-Users" }
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
                    options.UseInMemoryDatabase("InMemoryAuthTestDb");
                });

                // Mock HttpClient for User Service calls
                var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
                handlerMock
                    .Protected()
                    // Setup for /api/users/phone-exists/{phoneNumber}
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.PathAndQuery.Contains("/api/users/phone-exists/")),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = JsonContent.Create(new { exists = false }),
                    });

                handlerMock
                    .Protected()
                    // Setup for /api/users (POST profile)
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri!.PathAndQuery.Contains("/api/users")),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Created,
                        Content = JsonContent.Create(new { success = true }),
                    });

                var httpClient = new HttpClient(handlerMock.Object)
                {
                    BaseAddress = new Uri("http://user-service-mock:3001")
                };

                var mockFactory = new Mock<IHttpClientFactory>();
                mockFactory.Setup(_ => _.CreateClient("user")).Returns(httpClient);
                services.AddSingleton(mockFactory.Object);

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
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

    public async Task<string> RegisterAndLoginUser(string email, string password, string fullName, string phoneNumber, string address)
    {
        var registerDto = new RegisterDto
        {
            Email = email,
            Password = password,
            ConfirmPassword = password,
            FullName = fullName,
            PhoneNumber = phoneNumber,
            Address = address
        };

        var registerResponse = await Client.PostAsJsonAsync("/api/auth/register", registerDto);
        registerResponse.EnsureSuccessStatusCode();

        var loginDto = new LoginDto { Email = email, Password = password };
        var loginResponse = await Client.PostAsJsonAsync("/api/auth/login", loginDto);
        loginResponse.EnsureSuccessStatusCode();

        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        return authResponse!.Token;
    }
}



