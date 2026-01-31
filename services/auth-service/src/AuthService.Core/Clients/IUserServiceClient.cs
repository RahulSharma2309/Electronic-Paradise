using AuthService.Core.Clients.Models;

namespace AuthService.Core.Clients;

public interface IUserServiceClient
{
    Task<bool> PhoneExistsAsync(string phoneNumber, CancellationToken cancellationToken = default);

    Task CreateUserProfileAsync(
        CreateUserProfileRequest request,
        CancellationToken cancellationToken = default);
}
