// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterDto.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AuthService.Abstraction.DTOs;

/// <summary>
/// Data transfer object for user registration.
/// </summary>
public sealed record RegisterDto
{
    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Gets the user's password.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// Gets password confirmation for validation.
    /// </summary>
    public required string ConfirmPassword { get; init; }

    /// <summary>
    /// Gets the user's full name.
    /// </summary>
    public string? FullName { get; init; }

    /// <summary>
    /// Gets the user's phone number.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets the user's physical address.
    /// </summary>
    public string? Address { get; init; }
}
