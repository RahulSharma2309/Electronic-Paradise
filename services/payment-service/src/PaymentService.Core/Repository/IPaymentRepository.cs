using PaymentService.Abstraction.Models;

namespace PaymentService.Core.Repository;

/// <summary>
/// Defines the contract for payment record data access operations.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Adds a new payment record to the repository.
    /// </summary>
    /// <param name="payment">The payment record to add.</param>
    /// <returns>The added payment record with generated ID.</returns>
    Task<PaymentRecord> AddAsync(PaymentRecord payment);

    /// <summary>
    /// Retrieves a payment record by the associated order ID.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <returns>The payment record if found, otherwise null.</returns>
    Task<PaymentRecord?> GetByOrderIdAsync(Guid orderId);
}
