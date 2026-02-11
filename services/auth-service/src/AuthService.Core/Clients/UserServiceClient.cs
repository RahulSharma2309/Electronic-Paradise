using System.Net;
using System.Net.Http.Json;
using AuthService.Core.Clients.Models;
using Microsoft.Extensions.Logging;

namespace AuthService.Core.Clients;

public sealed class UserServiceClient : IUserServiceClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<UserServiceClient> _logger;

    public UserServiceClient(IHttpClientFactory httpClientFactory, ILogger<UserServiceClient> logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> PhoneExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number is required.", nameof(phoneNumber));
        }

        var client = _httpClientFactory.CreateClient("user");
        var url = $"api/users/phone-exists/{Uri.EscapeDataString(phoneNumber)}";

        try
        {
            using var res = await client.GetAsync(url, cancellationToken);
            if (!res.IsSuccessStatusCode)
            {
                _logger.LogWarning("UserService phone-exists returned {StatusCode}", res.StatusCode);
                throw new HttpRequestException("User service phone check failed.", null, res.StatusCode);
            }

            var payload = await res.Content.ReadFromJsonAsync<PhoneExistsResponse>(cancellationToken: cancellationToken);
            return payload?.Exists ?? false;
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed calling UserService phone-exists");
            throw new HttpRequestException("User service unavailable.", ex, HttpStatusCode.ServiceUnavailable);
        }
    }

    public async Task CreateUserProfileAsync(CreateUserProfileRequest request, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("user");

        try
        {
            using var res = await client.PostAsJsonAsync("api/users", request, cancellationToken);
            if (res.IsSuccessStatusCode)
            {
                return;
            }

            // Preserve the downstream status to allow proper mapping (409/400/etc).
            var body = await res.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning(
                "UserService profile create failed {StatusCode}. Body: {Body}",
                res.StatusCode,
                body);
            throw new HttpRequestException("User profile creation failed.", null, res.StatusCode);
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed calling UserService create profile");
            throw new HttpRequestException("User service unavailable.", ex, HttpStatusCode.ServiceUnavailable);
        }
    }

    private sealed record PhoneExistsResponse(bool Exists);
}
