using PaymentService.Abstraction.DTOs;
using PaymentService.Abstraction.Models;

namespace PaymentService.Core.Business;

/// <summary>
/// Defines the contract for payment-related business operations.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Processes a payment by debiting the user's wallet and recording the transaction.
    /// </summary>
    /// <param name="dto">The payment processing data.</param>
    /// <returns>The created payment record.</returns>
    /// <exception cref="ArgumentException">Thrown if the amount is not positive.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if there is insufficient wallet balance.</exception>
    /// <exception cref="HttpRequestException">Thrown if the wallet debit operation fails.</exception>
    Task<PaymentRecord> ProcessPaymentAsync(ProcessPaymentDto dto);

    /// <summary>
    /// Refunds a payment by crediting the user's wallet and recording the refund transaction.
    /// </summary>
    /// <param name="dto">The refund processing data.</param>
    /// <returns>The created refund payment record.</returns>
    /// <exception cref="ArgumentException">Thrown if the amount is not positive.</exception>
    /// <exception cref="HttpRequestException">Thrown if the wallet credit operation fails.</exception>
    Task<PaymentRecord> RefundPaymentAsync(RefundPaymentDto dto);

    /// <summary>
    /// Records a payment transaction (legacy endpoint for compatibility).
    /// </summary>
    /// <param name="dto">The payment record data.</param>
    /// <returns>The created payment record.</returns>
    Task<PaymentRecord> RecordPaymentAsync(RecordPaymentDto dto);

    /// <summary>
    /// Retrieves the payment status for a specific order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <returns>The payment record if found, otherwise null.</returns>
    Task<PaymentRecord?> GetPaymentStatusAsync(Guid orderId);
}
