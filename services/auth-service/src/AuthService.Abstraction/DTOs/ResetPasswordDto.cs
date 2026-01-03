// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetPasswordDto.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AuthService.Abstraction.DTOs;

/// <summary>
/// Data transfer object for resetting a user's password.
/// </summary>
public sealed record ResetPasswordDto
{
    /// <summary>
    /// Gets the email address of the user whose password is to be reset.
    /// </summary>
    required public string Email { get; init; }

    /// <summary>
    /// Gets the new password for the user.
    /// </summary>
    required public string NewPassword { get; init; }
}
