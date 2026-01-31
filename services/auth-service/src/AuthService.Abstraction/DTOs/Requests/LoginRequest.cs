using System.ComponentModel.DataAnnotations;

namespace AuthService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for user login.
/// Contains validation attributes for API layer.
/// </summary>
public record LoginRequest
{
    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    public string Email { get; init; } = null!;

    /// <summary>
    /// Gets the user's password.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public string Password { get; init; } = null!;
}
