namespace PaymentService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for refunding a payment.
/// </summary>
public sealed record RefundPaymentDto
{
    /// <summary>
    /// Gets the unique identifier of the order to refund.
    /// </summary>
    public required Guid OrderId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user from the Auth service.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user profile from the User service.
    /// </summary>
    public required Guid UserProfileId { get; init; }

    /// <summary>
    /// Gets the refund amount to credit back to the user's wallet.
    /// </summary>
    public required decimal Amount { get; init; }
}
