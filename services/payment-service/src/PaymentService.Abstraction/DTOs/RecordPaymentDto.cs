namespace PaymentService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for recording a payment (legacy endpoint).
/// </summary>
public sealed record RecordPaymentDto
{
    /// <summary>
    /// Gets the unique identifier of the order.
    /// </summary>
    public required Guid OrderId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Gets the payment amount.
    /// </summary>
    public required decimal Amount { get; init; }

    /// <summary>
    /// Gets the payment status.
    /// </summary>
    public required string Status { get; init; }
}
