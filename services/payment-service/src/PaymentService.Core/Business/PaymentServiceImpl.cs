using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PaymentService.Abstraction.DTOs;
using PaymentService.Abstraction.Models;
using PaymentService.Core.Repository;

namespace PaymentService.Core.Business;

/// <summary>
/// Provides implementation for payment-related business operations.
/// </summary>
public class PaymentServiceImpl : IPaymentService
{
    private readonly IPaymentRepository _repo;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PaymentServiceImpl> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentServiceImpl"/> class.
    /// </summary>
    /// <param name="repo">The payment repository.</param>
    /// <param name="httpClientFactory">The HTTP client factory for inter-service communication.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if any argument is null.</exception>
    public PaymentServiceImpl(
        IPaymentRepository repo,
        IHttpClientFactory httpClientFactory,
        ILogger<PaymentServiceImpl> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<PaymentRecord> ProcessPaymentAsync(ProcessPaymentDto dto)
    {
        _logger.LogInformation("Processing payment for Order {OrderId}, UserId: {UserId}, Amount: {Amount}", dto.OrderId, dto.UserId, dto.Amount);

        if (dto.Amount <= 0)
        {
            _logger.LogWarning("Payment processing failed for Order {OrderId}: Amount must be greater than 0. Amount: {Amount}", dto.OrderId, dto.Amount);
            throw new ArgumentException("Amount must be greater than 0", nameof(dto.Amount));
        }

        try
        {
            var userClient = _httpClientFactory.CreateClient("user");

            // 1) Attempt to debit wallet via User Service
            var debitResponse = await userClient.PostAsJsonAsync(
                $"/api/users/{dto.UserProfileId}/wallet/debit",
                new { Amount = dto.Amount });

            if (debitResponse.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Payment processing failed for Order {OrderId}: User profile {UserProfileId} not found.", dto.OrderId, dto.UserProfileId);
                throw new KeyNotFoundException("User not found");
            }

            if (debitResponse.StatusCode == HttpStatusCode.Conflict)
            {
                _logger.LogWarning("Payment processing failed for Order {OrderId}: Insufficient wallet balance for user {UserId}.", dto.OrderId, dto.UserId);
                throw new InvalidOperationException("Insufficient wallet balance");
            }

            if (!debitResponse.IsSuccessStatusCode)
            {
                _logger.LogError("User Service wallet debit failed for Order {OrderId} with status {StatusCode}", dto.OrderId, debitResponse.StatusCode);
                throw new HttpRequestException($"Wallet debit failed with status {debitResponse.StatusCode}");
            }

            // 2) Wallet debited successfully - now record payment
            var payment = new PaymentRecord
            {
                OrderId = dto.OrderId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                Status = "Paid",
                Timestamp = DateTime.UtcNow,
            };

            await _repo.AddAsync(payment);

            _logger.LogInformation("Payment processed successfully for Order {OrderId}, Payment ID: {PaymentId}, Amount: {Amount}", dto.OrderId, payment.Id, dto.Amount);

            return payment;
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException && ex is not HttpRequestException)
        {
            _logger.LogError(ex, "Unexpected error processing payment for Order {OrderId}", dto.OrderId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentRecord> RefundPaymentAsync(RefundPaymentDto dto)
    {
        _logger.LogInformation("Processing refund for Order {OrderId}, UserId: {UserId}, Amount: {Amount}", dto.OrderId, dto.UserId, dto.Amount);

        if (dto.Amount <= 0)
        {
            _logger.LogWarning("Refund processing failed for Order {OrderId}: Amount must be greater than 0. Amount: {Amount}", dto.OrderId, dto.Amount);
            throw new ArgumentException("Amount must be greater than 0", nameof(dto.Amount));
        }

        try
        {
            var userClient = _httpClientFactory.CreateClient("user");

            // Credit the wallet back
            var creditResponse = await userClient.PostAsJsonAsync(
                $"/api/users/{dto.UserProfileId}/wallet/credit",
                new { Amount = dto.Amount });

            if (!creditResponse.IsSuccessStatusCode)
            {
                _logger.LogError("User Service wallet credit (refund) failed for Order {OrderId} with status {StatusCode}", dto.OrderId, creditResponse.StatusCode);
                throw new HttpRequestException($"Wallet refund failed with status {creditResponse.StatusCode}");
            }

            // Record refund payment entry
            var refundPayment = new PaymentRecord
            {
                OrderId = dto.OrderId,
                UserId = dto.UserId,
                Amount = -dto.Amount, // Negative to indicate refund
                Status = "Refunded",
                Timestamp = DateTime.UtcNow,
            };

            await _repo.AddAsync(refundPayment);

            _logger.LogInformation("Refund processed successfully for Order {OrderId}, Payment ID: {PaymentId}, Amount: {Amount}", dto.OrderId, refundPayment.Id, dto.Amount);

            return refundPayment;
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not HttpRequestException)
        {
            _logger.LogError(ex, "Unexpected error processing refund for Order {OrderId}", dto.OrderId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentRecord> RecordPaymentAsync(RecordPaymentDto dto)
    {
        _logger.LogInformation("Recording payment for Order {OrderId}, Status: {Status}", dto.OrderId, dto.Status);

        try
        {
            // Legacy endpoint - kept for compatibility
            var payment = new PaymentRecord
            {
                OrderId = dto.OrderId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                Status = dto.Status,
                Timestamp = DateTime.UtcNow,
            };

            var result = await _repo.AddAsync(payment);
            _logger.LogInformation("Payment recorded successfully for Order {OrderId}, Payment ID: {PaymentId}", dto.OrderId, result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording payment for Order {OrderId}", dto.OrderId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentRecord?> GetPaymentStatusAsync(Guid orderId)
    {
        _logger.LogDebug("Fetching payment status for Order {OrderId}", orderId);

        try
        {
            var payment = await _repo.GetByOrderIdAsync(orderId);
            if (payment == null)
            {
                _logger.LogWarning("Payment record not found for Order {OrderId}", orderId);
            }
            else
            {
                _logger.LogDebug("Payment record found for Order {OrderId}: Status {Status}", orderId, payment.Status);
            }

            return payment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching payment status for Order {OrderId}", orderId);
            throw;
        }
    }
}
