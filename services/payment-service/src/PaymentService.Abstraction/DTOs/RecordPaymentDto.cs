namespace PaymentService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for recording a payment (legacy endpoint).
/// </summary>
public sealed record RecordPaymentDto
{
    /// <summary>
    /// Gets the unique identifier of the order.
    /// </summary>
    required public Guid OrderId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    required public Guid UserId { get; init; }

    /// <summary>
    /// Gets the payment amount.
    /// </summary>
    required public decimal Amount { get; init; }

    /// <summary>
    /// Gets the payment status.
    /// </summary>
    required public string Status { get; init; }
}
