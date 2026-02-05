// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginDto.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
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
    public required string Email { get; init; }

    /// <summary>
    /// Gets the user's password.
    /// </summary>
    public required string Password { get; init; }
}
