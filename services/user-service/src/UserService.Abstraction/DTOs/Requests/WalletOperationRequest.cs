using System.ComponentModel.DataAnnotations;

namespace UserService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for wallet operations (debit/credit).
/// Used for payment processing integrations.
/// </summary>
public record WalletOperationRequest
{
    /// <summary>
    /// Gets the amount for the wallet operation.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; init; }

    /// <summary>
    /// Gets the operation type.
    /// Values: "credit" or "debit".
    /// </summary>
    [Required(ErrorMessage = "Operation type is required")]
    [RegularExpression("^(credit|debit)$", ErrorMessage = "Operation type must be 'credit' or 'debit'")]
    public string OperationType { get; init; } = null!;
}
