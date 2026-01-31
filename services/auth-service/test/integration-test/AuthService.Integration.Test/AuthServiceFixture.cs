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
using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.DTOs.Responses;
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
                conf.AddInMemoryCollection(new Dictionary<string, string?>
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
                        var uri = request.RequestUri?.ToString() ?? string.Empty;

                        // Mock phone-exists check - always return false (phone doesn't exist)
                        if (request.Method == HttpMethod.Get && uri.Contains("phone-exists"))
                        {
                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = JsonContent.Create(new { exists = false })
                            };
                        }

                        // Mock user profile creation - always return success
                        if (request.Method == HttpMethod.Post && uri.Contains("api/users"))
                        {
                            return new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.Created,
                                Content = JsonContent.Create(new { success = true })
                            };
                        }

                        // Default response for any other User Service calls
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json")
                        };
                    });

                var httpClient = new HttpClient(handlerMock.Object)
                {
                    BaseAddress = new Uri("http://user-service-mock:3001")
                };

                var mockFactory = new Mock<IHttpClientFactory>();
                mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
                services.AddSingleton(mockFactory.Object);
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
        var registerDto = new RegisterRequest
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

        var loginDto = new LoginRequest { Email = email, Password = password };
        var loginResponse = await Client.PostAsJsonAsync("/api/auth/login", loginDto);
        loginResponse.EnsureSuccessStatusCode();

        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        return authResponse!.Token;
    }
}



