using System.ComponentModel.DataAnnotations;

namespace AuthService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for updating user information.
/// All fields are optional - only provided fields will be updated (PATCH semantics).
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the user's full name.
    /// </summary>
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 200 characters")]
    public string? FullName { get; set; }

    /// <summary>
    /// Gets or sets the current password (required if changing password).
    /// </summary>
    public string? CurrentPassword { get; set; }

    /// <summary>
    /// Gets or sets the new password (if changing password).
    /// </summary>
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public string? NewPassword { get; set; }
}
