namespace PaymentService.Abstraction.Models;

/// <summary>
/// Represents a payment record in the Payment service.
/// </summary>
public class PaymentRecord
{
    /// <summary>
    /// Gets or sets the unique identifier for the payment record.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the order identifier associated with this payment.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier who made the payment.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the payment amount. Negative values indicate refunds.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the payment status (e.g., "Paid", "Refunded", "Failed").
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the payment was recorded.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
