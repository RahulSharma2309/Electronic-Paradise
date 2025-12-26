using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Dtos;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(AppDbContext db, IHttpClientFactory httpClientFactory, ILogger<PaymentsController> logger)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }


    // POST: api/payments/process
    // Body: { "orderId": "...", "userId": "...", "userProfileId": "...", "amount": 100 }
    // Processes payment by debiting wallet via User Service and recording transaction
    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment(ProcessPaymentDto dto)
    {
        if (dto.Amount <= 0) 
            return BadRequest(new { error = "Amount must be greater than 0" });

        var userClient = _httpClientFactory.CreateClient("user");

        // 1) Attempt to debit wallet via User Service
        try
        {
            var debitResponse = await userClient.PostAsJsonAsync(
                $"/api/users/{dto.UserProfileId}/wallet/debit", 
                new { Amount = dto.Amount }
            );

            if (debitResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new { error = "User not found" });
            }

            if (debitResponse.StatusCode == HttpStatusCode.Conflict)
            {
                var conflictBody = await debitResponse.Content.ReadFromJsonAsync<object>();
                return Conflict(new { error = "Insufficient wallet balance", details = conflictBody });
            }

            if (!debitResponse.IsSuccessStatusCode)
            {
                _logger.LogError("User Service wallet debit failed with status {StatusCode}", debitResponse.StatusCode);
                return StatusCode((int)debitResponse.StatusCode, new { error = "Wallet debit failed" });
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

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Payment processed successfully for Order {OrderId}, Amount: {Amount}", dto.OrderId, dto.Amount);

            return Ok(new 
            { 
                paymentId = payment.Id,
                orderId = payment.OrderId, 
                userId = payment.UserId, 
                amount = payment.Amount, 
                status = payment.Status, 
                timestamp = payment.Timestamp 
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with User Service for payment processing");
            return StatusCode(503, new { error = "User Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Payment processing failed for Order {OrderId}", dto.OrderId);
            return StatusCode(500, new { error = "Payment processing failed" });
        }
    }

    // POST: api/payments/refund
    // Body: { "orderId": "...", "userProfileId": "...", "amount": 100 }
    // Refunds payment by crediting wallet via User Service
    [HttpPost("refund")]
    public async Task<IActionResult> RefundPayment(RefundPaymentDto dto)
    {
        if (dto.Amount <= 0) 
            return BadRequest(new { error = "Amount must be greater than 0" });

        var userClient = _httpClientFactory.CreateClient("user");

        try
        {
            // Credit the wallet back
            var creditResponse = await userClient.PostAsJsonAsync(
                $"/api/users/{dto.UserProfileId}/wallet/credit", 
                new { Amount = dto.Amount }
            );

            if (!creditResponse.IsSuccessStatusCode)
            {
                _logger.LogError("User Service wallet credit (refund) failed with status {StatusCode}", creditResponse.StatusCode);
                return StatusCode((int)creditResponse.StatusCode, new { error = "Wallet refund failed" });
            }

            // Record refund payment entry (optional - could also update existing payment status)
            var refundPayment = new PaymentRecord
            {
                OrderId = dto.OrderId,
                UserId = dto.UserId,
                Amount = -dto.Amount, // Negative to indicate refund
                Status = "Refunded",
                Timestamp = DateTime.UtcNow
            };

            _db.Payments.Add(refundPayment);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Payment refunded successfully for Order {OrderId}, Amount: {Amount}", dto.OrderId, dto.Amount);

            return Ok(new 
            { 
                paymentId = refundPayment.Id,
                orderId = refundPayment.OrderId, 
                userId = refundPayment.UserId, 
                amount = refundPayment.Amount, 
                status = refundPayment.Status, 
                timestamp = refundPayment.Timestamp 
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with User Service for refund processing");
            return StatusCode(503, new { error = "User Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Refund processing failed for Order {OrderId}", dto.OrderId);
            return StatusCode(500, new { error = "Refund processing failed" });
        }
    }

    // POST: api/payments/record
    // Body: { "orderId": "...", "userId": "...", "amount": 100, "status": "Paid" }
    [HttpPost("record")]
    public async Task<IActionResult> RecordPayment(RecordPaymentDto dto)
    {
        // Simulate recording a payment for an order (legacy endpoint - kept for compatibility)
        var payment = new PaymentRecord
        {
            OrderId = dto.OrderId,
            UserId = dto.UserId,
            Amount = dto.Amount,
            Status = dto.Status,
            Timestamp = DateTime.UtcNow
        };
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();
        return Ok(new { payment.OrderId, payment.UserId, payment.Amount, payment.Status, payment.Timestamp });
    }

    // GET: api/payments/status/{orderId}
    [HttpGet("status/{orderId}")]
    public async Task<IActionResult> GetPaymentStatus(Guid orderId)
    {
        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (payment == null) return NotFound();
        return Ok(new { payment.OrderId, payment.Status, payment.Timestamp });
    }
}

public record CreateWalletDto(Guid UserId, int InitialBalance);
public record DebitDto(Guid UserId, int Amount);
public record CreditDto(Guid UserId, int Amount);
