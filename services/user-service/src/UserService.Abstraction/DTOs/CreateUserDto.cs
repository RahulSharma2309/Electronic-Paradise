// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateUserDto.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UserService.Abstraction.DTOs;

/// <summary>
/// Data transfer object for creating a new user profile.
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// Gets or sets the user ID from the authentication service.
    /// This links the profile to the identity provider's user account.
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
    /// Gets or sets the user's physical address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the user's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }
}
