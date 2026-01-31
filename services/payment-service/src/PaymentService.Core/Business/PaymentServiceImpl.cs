using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PaymentService.Abstraction.DTOs.Requests;
using PaymentService.Abstraction.DTOs.Responses;
using PaymentService.Abstraction.Models;
using PaymentService.Core.Mappers;
using PaymentService.Core.Repository;

namespace PaymentService.Core.Business;

/// <summary>
/// Provides implementation for payment-related business operations.
/// </summary>
public class PaymentServiceImpl : IPaymentService
{
    private readonly IPaymentRepository _repo;
    private readonly IPaymentMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PaymentServiceImpl> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentServiceImpl"/> class.
    /// </summary>
    /// <param name="repo">The payment repository.</param>
    /// <param name="mapper">The payment mapper.</param>
    /// <param name="httpClientFactory">The HTTP client factory for inter-service communication.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if any argument is null.</exception>
    public PaymentServiceImpl(
        IPaymentRepository repo,
        IPaymentMapper mapper,
        IHttpClientFactory httpClientFactory,
        ILogger<PaymentServiceImpl> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<PaymentDetailResponse> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        if (request.OrderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID is required", nameof(request.OrderId));
        }

        if (request.UserId == Guid.Empty)
        {
            throw new ArgumentException("User ID is required", nameof(request.UserId));
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0", nameof(request.Amount));
        }

        _logger.LogInformation("Processing payment for Order {OrderId}, UserId: {UserId}, Amount: {Amount}", request.OrderId, request.UserId, request.Amount);

        try
        {
            var userClient = _httpClientFactory.CreateClient("user");

            // 1) Attempt to debit wallet via User Service
            var debitResponse = await userClient.PostAsJsonAsync(
                $"/api/users/{request.UserId}/wallet/debit",
                new
                {
                    Amount = request.Amount,
                    OperationType = "debit",
                });

            if (debitResponse.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Payment processing failed for Order {OrderId}: User not found.", request.OrderId);
                throw new KeyNotFoundException("User not found");
            }

            if (debitResponse.StatusCode == HttpStatusCode.Conflict)
            {
                _logger.LogWarning("Payment processing failed for Order {OrderId}: Insufficient wallet balance for user {UserId}.", request.OrderId, request.UserId);
                throw new InvalidOperationException("Insufficient wallet balance");
            }

            if (!debitResponse.IsSuccessStatusCode)
            {
                _logger.LogError("User Service wallet debit failed for Order {OrderId} with status {StatusCode}", request.OrderId, debitResponse.StatusCode);
                throw new HttpRequestException($"Wallet debit failed with status {debitResponse.StatusCode}");
            }

            // 2) Wallet debited successfully - now record payment
            var payment = _mapper.ToEntity(request, PaymentStatus.Success.ToString());
            await _repo.AddAsync(payment);

            _logger.LogInformation("Payment processed successfully for Order {OrderId}, Payment ID: {PaymentId}, Amount: {Amount}", request.OrderId, payment.Id, request.Amount);

            return _mapper.ToDetailResponse(payment);
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException && ex is not HttpRequestException)
        {
            _logger.LogError(ex, "Unexpected error processing payment for Order {OrderId}", request.OrderId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentDetailResponse> RefundPaymentAsync(RefundPaymentRequest request)
    {
        if (request.PaymentId == Guid.Empty)
        {
            throw new ArgumentException("Payment ID is required", nameof(request.PaymentId));
        }

        _logger.LogInformation("Processing refund for Payment {PaymentId}, Reason: {Reason}", request.PaymentId, request.Reason);

        try
        {
            // Get original payment
            var originalPayment = await _repo.GetByIdAsync(request.PaymentId);
            if (originalPayment == null)
            {
                _logger.LogWarning("Refund failed: Payment {PaymentId} not found", request.PaymentId);
                throw new KeyNotFoundException($"Payment {request.PaymentId} not found");
            }

            var userClient = _httpClientFactory.CreateClient("user");

            // Credit the wallet back
            var creditResponse = await userClient.PostAsJsonAsync(
                $"/api/users/{originalPayment.UserId}/wallet/credit",
                new
                {
                    Amount = originalPayment.Amount,
                    OperationType = "credit",
                });

            if (!creditResponse.IsSuccessStatusCode)
            {
                _logger.LogError("User Service wallet credit (refund) failed for Payment {PaymentId} with status {StatusCode}", request.PaymentId, creditResponse.StatusCode);
                throw new HttpRequestException($"Wallet refund failed with status {creditResponse.StatusCode}");
            }

            // Update original payment status
            originalPayment.Status = PaymentStatus.Refunded.ToString();
            originalPayment.Timestamp = DateTime.UtcNow;
            await _repo.UpdateAsync(originalPayment);

            _logger.LogInformation("Refund processed successfully for Payment {PaymentId}, Amount: {Amount}", request.PaymentId, originalPayment.Amount);

            return _mapper.ToDetailResponse(originalPayment);
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException && ex is not HttpRequestException)
        {
            _logger.LogError(ex, "Unexpected error processing refund for Payment {PaymentId}", request.PaymentId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentDetailResponse?> GetPaymentByIdAsync(Guid paymentId)
    {
        _logger.LogDebug("Fetching payment by ID: {PaymentId}", paymentId);

        try
        {
            var payment = await _repo.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning("Payment not found: {PaymentId}", paymentId);
                return null;
            }

            return _mapper.ToDetailResponse(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching payment {PaymentId}", paymentId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentStatusResponse?> GetPaymentStatusAsync(Guid orderId)
    {
        _logger.LogDebug("Fetching payment status for Order {OrderId}", orderId);

        try
        {
            var payment = await _repo.GetByOrderIdAsync(orderId);
            if (payment == null)
            {
                _logger.LogWarning("Payment record not found for Order {OrderId}", orderId);
                return null;
            }

            _logger.LogDebug("Payment record found for Order {OrderId}: Status {Status}", orderId, payment.Status);
            return _mapper.ToStatusResponse(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching payment status for Order {OrderId}", orderId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<PaymentResponse>> GetUserPaymentsAsync(Guid userId)
    {
        _logger.LogDebug("Fetching payments for User {UserId}", userId);

        try
        {
            var payments = await _repo.GetByUserIdAsync(userId);
            _logger.LogInformation("Retrieved {Count} payments for User {UserId}", payments.Count, userId);
            return payments.Select(_mapper.ToResponse).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching payments for User {UserId}", userId);
            throw;
        }
    }
}
