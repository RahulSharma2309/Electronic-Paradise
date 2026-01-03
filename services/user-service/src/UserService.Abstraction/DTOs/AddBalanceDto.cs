// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddBalanceDto.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UserService.Abstraction.DTOs;

/// <summary>
/// Data transfer object for adding balance to a user's wallet.
/// </summary>
public sealed record AddBalanceDto
{
    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    required public Guid UserId { get; init; }

    /// <summary>
    /// Gets the amount to add to the wallet balance.
    /// </summary>
    required public decimal Amount { get; init; }
}
