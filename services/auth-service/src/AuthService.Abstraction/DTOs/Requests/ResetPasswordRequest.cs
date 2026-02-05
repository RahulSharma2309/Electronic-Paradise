using System.ComponentModel.DataAnnotations;

namespace AuthService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for password reset.
/// Contains validation attributes for API layer.
/// </summary>
public record ResetPasswordRequest
{
    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; init; } = null!;

    /// <summary>
    /// Gets the password reset token.
    /// </summary>
    [Required(ErrorMessage = "Reset token is required")]
    public string ResetToken { get; init; } = null!;

    /// <summary>
    /// Gets the new password.
    /// </summary>
    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit")]
    public string NewPassword { get; init; } = null!;
}
