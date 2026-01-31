namespace UserService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for user profile (lightweight).
/// Used for list views or quick profile lookups.
/// </summary>
public class UserProfileResponse
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID from auth service.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets the full name (computed).
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Gets or sets the wallet balance.
    /// </summary>
    public decimal WalletBalance { get; set; }
}
