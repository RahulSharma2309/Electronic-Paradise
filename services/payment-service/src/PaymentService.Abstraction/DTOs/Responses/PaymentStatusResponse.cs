namespace PaymentService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for payment status checks.
/// Lightweight DTO for quick status lookups.
/// </summary>
public class PaymentStatusResponse
{
    /// <summary>
    /// Gets or sets the payment ID.
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Gets or sets the order ID.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Gets or sets the current status.
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Gets or sets a value indicating whether the payment is successful.
    /// Convenience flag for quick checks.
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of last status update.
    /// </summary>
    public DateTime LastUpdated { get; set; }
}
