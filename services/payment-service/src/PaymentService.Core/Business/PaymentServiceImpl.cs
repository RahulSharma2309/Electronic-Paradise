using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PaymentService.Abstraction.DTOs;
using PaymentService.Abstraction.Models;
using PaymentService.Core.Repository;

namespace PaymentService.Core.Business;

public class PaymentServiceImpl : IPaymentService
{
    private readonly IPaymentRepository _repo;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PaymentServiceImpl> _logger;

    public PaymentServiceImpl(
        IPaymentRepository repo, 
        IHttpClientFactory httpClientFactory, 
        ILogger<PaymentServiceImpl> logger)
    {
        _repo = repo;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<PaymentRecord> ProcessPaymentAsync(ProcessPaymentDto dto)
    {
        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");

        var userClient = _httpClientFactory.CreateClient("user");

        // 1) Attempt to debit wallet via User Service
        var debitResponse = await userClient.PostAsJsonAsync(
            $"/api/users/{dto.UserProfileId}/wallet/debit", 
            new { Amount = dto.Amount }
        );

        if (debitResponse.StatusCode == HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException("User not found");
        }

        if (debitResponse.StatusCode == HttpStatusCode.Conflict)
        {
            throw new InvalidOperationException("Insufficient wallet balance");
        }

        if (!debitResponse.IsSuccessStatusCode)
        {
            _logger.LogError("User Service wallet debit failed with status {StatusCode}", debitResponse.StatusCode);
            throw new HttpRequestException($"Wallet debit failed with status {debitResponse.StatusCode}");
        }

        // 2) Wallet debited successfully - now record payment
        var payment = new PaymentRecord
        {
            OrderId = dto.OrderId,
            UserId = dto.UserId,
            Amount = dto.Amount,
            Status = "Paid",
            Timestamp = DateTime.UtcNow
        };

        await _repo.AddAsync(payment);

        _logger.LogInformation("Payment processed successfully for Order {OrderId}, Amount: {Amount}", dto.OrderId, dto.Amount);

        return payment;
    }

    public async Task<PaymentRecord> RefundPaymentAsync(RefundPaymentDto dto)
    {
        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");

        var userClient = _httpClientFactory.CreateClient("user");

        // Credit the wallet back
        var creditResponse = await userClient.PostAsJsonAsync(
            $"/api/users/{dto.UserProfileId}/wallet/credit", 
            new { Amount = dto.Amount }
        );

        if (!creditResponse.IsSuccessStatusCode)
        {
            _logger.LogError("User Service wallet credit (refund) failed with status {StatusCode}", creditResponse.StatusCode);
            throw new HttpRequestException($"Wallet refund failed with status {creditResponse.StatusCode}");
        }

        // Record refund payment entry
        var refundPayment = new PaymentRecord
        {
            OrderId = dto.OrderId,
            UserId = dto.UserId,
            Amount = -dto.Amount, // Negative to indicate refund
            Status = "Refunded",
            Timestamp = DateTime.UtcNow
        };

        await _repo.AddAsync(refundPayment);

        _logger.LogInformation("Payment refunded successfully for Order {OrderId}, Amount: {Amount}", dto.OrderId, dto.Amount);

        return refundPayment;
    }

    public async Task<PaymentRecord> RecordPaymentAsync(RecordPaymentDto dto)
    {
        // Legacy endpoint - kept for compatibility
        var payment = new PaymentRecord
        {
            OrderId = dto.OrderId,
            UserId = dto.UserId,
            Amount = dto.Amount,
            Status = dto.Status,
            Timestamp = DateTime.UtcNow
        };

        return await _repo.AddAsync(payment);
    }

    public async Task<PaymentRecord?> GetPaymentStatusAsync(Guid orderId)
    {
        return await _repo.GetByOrderIdAsync(orderId);
    }
}




