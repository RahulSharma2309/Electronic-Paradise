using System.ComponentModel.DataAnnotations;

namespace AuthService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for user registration.
/// Contains validation attributes for API layer.
/// </summary>
public record RegisterRequest
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
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
    public string Password { get; init; } = null!;

    /// <summary>
    /// Gets the confirmation password (must match <see cref="Password"/>).
    /// </summary>
    [Required(ErrorMessage = "Confirm password is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; init; } = null!;

    /// <summary>
    /// Gets the user's full name.
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 200 characters")]
    public string FullName { get; init; } = null!;

    /// <summary>
    /// Gets the user's phone number.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(50, ErrorMessage = "Phone number cannot exceed 50 characters")]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; init; } = null!;

    /// <summary>
    /// Gets the user's address (optional at registration).
    /// </summary>
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; init; }
}
