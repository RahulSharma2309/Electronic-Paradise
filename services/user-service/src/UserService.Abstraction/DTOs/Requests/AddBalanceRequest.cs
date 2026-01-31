using System.ComponentModel.DataAnnotations;

namespace UserService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for adding balance to a user's wallet.
/// </summary>
public record AddBalanceRequest
{
    /// <summary>
    /// Gets the amount to add to the wallet balance.
    /// Must be positive.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; init; }
}
