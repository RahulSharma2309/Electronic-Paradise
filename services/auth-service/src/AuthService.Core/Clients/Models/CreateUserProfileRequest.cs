namespace AuthService.Core.Clients.Models;

public sealed record CreateUserProfileRequest(
    Guid UserId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Address);
