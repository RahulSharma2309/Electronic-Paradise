namespace AuthService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for user information (lightweight).
/// Used within AuthResponse and for quick user lookups.
/// </summary>
public class UserResponse
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's full name.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
