using PaymentService.Abstraction.DTOs.Requests;
using PaymentService.Abstraction.DTOs.Responses;

namespace PaymentService.Core.Business;

/// <summary>
/// Defines the contract for payment-related business operations.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Processes a payment by debiting the user's wallet and recording the transaction.
    /// </summary>
    /// <param name="request">The payment processing request.</param>
    /// <returns>The created payment with comprehensive details.</returns>
    /// <exception cref="ArgumentException">Thrown if the amount is not positive.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if there is insufficient wallet balance.</exception>
    /// <exception cref="HttpRequestException">Thrown if the wallet debit operation fails.</exception>
    Task<PaymentDetailResponse> ProcessPaymentAsync(ProcessPaymentRequest request);

    /// <summary>
    /// Refunds a payment by crediting the user's wallet and recording the refund transaction.
    /// </summary>
    /// <param name="request">The refund processing request.</param>
    /// <returns>The created refund payment with comprehensive details.</returns>
    /// <exception cref="ArgumentException">Thrown if the amount is not positive.</exception>
    /// <exception cref="HttpRequestException">Thrown if the wallet credit operation fails.</exception>
    Task<PaymentDetailResponse> RefundPaymentAsync(RefundPaymentRequest request);

    /// <summary>
    /// Retrieves a payment by its unique identifier (comprehensive details).
    /// </summary>
    /// <param name="id">The unique identifier of the payment.</param>
    /// <returns>The payment if found, otherwise null.</returns>
    Task<PaymentDetailResponse?> GetPaymentByIdAsync(Guid id);

    /// <summary>
    /// Retrieves the payment status for a specific order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <returns>The payment status if found, otherwise null.</returns>
    Task<PaymentStatusResponse?> GetPaymentStatusAsync(Guid orderId);

    /// <summary>
    /// Retrieves all payments for a specific user (lightweight for lists).
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of payments for the user.</returns>
    Task<List<PaymentResponse>> GetUserPaymentsAsync(Guid userId);
}
