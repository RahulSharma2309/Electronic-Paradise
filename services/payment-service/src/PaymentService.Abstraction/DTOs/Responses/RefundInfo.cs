namespace PaymentService.Abstraction.DTOs.Responses;

/// <summary>
/// Represents refund information for a payment.
/// </summary>
public class RefundInfo
{
    /// <summary>
    /// Gets or sets the refund amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the refund timestamp.
    /// </summary>
    public DateTime RefundedAt { get; set; }

    /// <summary>
    /// Gets or sets the refund reason.
    /// </summary>
    public string? Reason { get; set; }
}
