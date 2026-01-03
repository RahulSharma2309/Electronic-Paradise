namespace PaymentService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for refunding a payment.
/// </summary>
public sealed record RefundPaymentDto
{
    /// <summary>
    /// Gets the unique identifier of the order to refund.
    /// </summary>
    required public Guid OrderId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user from the Auth service.
    /// </summary>
    required public Guid UserId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the user profile from the User service.
    /// </summary>
    required public Guid UserProfileId { get; init; }

    /// <summary>
    /// Gets the refund amount to credit back to the user's wallet.
    /// </summary>
    required public decimal Amount { get; init; }
}
