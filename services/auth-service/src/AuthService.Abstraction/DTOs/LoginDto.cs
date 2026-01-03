// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginDto.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AuthService.Abstraction.DTOs;

/// <summary>
/// Data transfer object for user login.
/// </summary>
public sealed record LoginDto
{
    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the user's password.
    /// </summary>
    required public string Password { get; init; }
}
