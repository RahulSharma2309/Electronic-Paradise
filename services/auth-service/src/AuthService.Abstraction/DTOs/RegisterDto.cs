// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterDto.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
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
    required public string Email { get; init; }

    /// <summary>
    /// Gets the user's password.
    /// </summary>
    required public string Password { get; init; }

    /// <summary>
    /// Gets password confirmation for validation.
    /// </summary>
    required public string ConfirmPassword { get; init; }

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
