// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfile.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UserService.Abstraction.Models;

/// <summary>
/// Represents a user profile containing personal information and wallet balance.
/// Clean domain model without infrastructure concerns (EF Core attributes removed).
/// Configuration moved to Fluent API in AppDbContext.
/// </summary>
public class UserProfile
{
    /// <summary>
    /// Gets or sets the unique identifier for the user profile.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

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

    /// <summary>
    /// Gets or sets the timestamp when the profile was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp when the profile was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the user's wallet balance for e-commerce transactions.
    /// Stored as decimal(18,2) in database (configured via Fluent API).
    /// </summary>
    public decimal WalletBalance { get; set; } = 0;
}
