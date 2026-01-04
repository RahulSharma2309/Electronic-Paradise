namespace OrderService.Abstraction.DTOs;

/// <summary>
/// Represents user profile information from the User service.
/// </summary>
public sealed record UserProfileDto
{
    /// <summary>
    /// Gets the unique identifier of the user profile.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user from the Auth service.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Gets the first name of the user.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// Gets the last name of the user.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// Gets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets the wallet balance of the user.
    /// </summary>
    public required decimal WalletBalance { get; init; }
}
