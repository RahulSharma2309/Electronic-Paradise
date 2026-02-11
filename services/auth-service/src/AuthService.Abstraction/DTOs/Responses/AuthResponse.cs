namespace AuthService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for authentication operations (login, register).
/// Contains JWT token and user information.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Gets or sets the JWT access token.
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// Gets or sets the token expiration timestamp.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the authenticated user information.
    /// </summary>
    public UserResponse User { get; set; } = null!;
}
