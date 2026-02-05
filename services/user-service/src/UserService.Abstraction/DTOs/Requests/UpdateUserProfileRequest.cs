using System.ComponentModel.DataAnnotations;

namespace UserService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for updating an existing user profile.
/// All fields are optional - only provided fields will be updated (PATCH semantics).
/// </summary>
public class UpdateUserProfileRequest
{
    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the user's physical address.
    /// </summary>
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the user's phone number.
    /// </summary>
    [StringLength(50, ErrorMessage = "Phone number cannot exceed 50 characters")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }
}
