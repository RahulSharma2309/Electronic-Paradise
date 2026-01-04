// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WalletOperationDto.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UserService.Abstraction.DTOs;

/// <summary>
/// Data transfer object for wallet operations (debit/credit).
/// </summary>
public sealed record WalletOperationDto
{
    /// <summary>
    /// Gets the amount to debit or credit from the wallet.
    /// </summary>
    public required decimal Amount { get; init; }
}
