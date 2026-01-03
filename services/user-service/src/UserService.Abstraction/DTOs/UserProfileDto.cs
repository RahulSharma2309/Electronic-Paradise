// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileDto.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UserService.Abstraction.Models;

namespace UserService.Abstraction.DTOs;

/// <summary>
/// Data transfer object for user profile information.
/// </summary>
public class UserProfileDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the user profile.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID from the authentication service.
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

    /// <summary>
    /// Gets or sets the timestamp when the profile was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the profile was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user's wallet balance.
    /// </summary>
    public decimal WalletBalance { get; set; }

    /// <summary>
    /// Converts a <see cref="UserProfile"/> model to a <see cref="UserProfileDto"/>.
    /// </summary>
    /// <param name="m">The user profile model to convert.</param>
    /// <returns>A <see cref="UserProfileDto"/> containing the model's data.</returns>
    public static UserProfileDto FromModel(UserProfile m) => new UserProfileDto
    {
        Id = m.Id,
        UserId = m.UserId,
        FirstName = m.FirstName,
        LastName = m.LastName,
        Address = m.Address,
        PhoneNumber = m.PhoneNumber,
        CreatedAt = m.CreatedAt,
        UpdatedAt = m.UpdatedAt,
        WalletBalance = m.WalletBalance
    };
}
