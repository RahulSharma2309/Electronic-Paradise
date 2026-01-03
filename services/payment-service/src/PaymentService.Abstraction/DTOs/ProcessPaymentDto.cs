namespace PaymentService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for processing a payment.
/// </summary>
public sealed record ProcessPaymentDto
{
    /// <summary>
    /// Gets the unique identifier of the order.
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
    /// Gets the payment amount to deduct from the user's wallet.
    /// </summary>
    required public decimal Amount { get; init; }
}
