using Microsoft.AspNetCore.Mvc;
using PaymentService.Abstraction.DTOs;
using PaymentService.Core.Business;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _service;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService service, ILogger<PaymentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // POST: api/payments/process
    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment(ProcessPaymentDto dto)
    {
        try
        {
            var payment = await _service.ProcessPaymentAsync(dto);
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
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = "User not found" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with User Service");
            return StatusCode(503, new { error = "User Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Payment processing failed for Order {OrderId}", dto.OrderId);
            return StatusCode(500, new { error = "Payment processing failed" });
        }
    }

    // POST: api/payments/refund
    [HttpPost("refund")]
    public async Task<IActionResult> RefundPayment(RefundPaymentDto dto)
    {
        try
        {
            var payment = await _service.RefundPaymentAsync(dto);
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
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with User Service for refund");
            return StatusCode(503, new { error = "User Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Refund processing failed for Order {OrderId}", dto.OrderId);
            return StatusCode(500, new { error = "Refund processing failed" });
        }
    }

    // POST: api/payments/record
    [HttpPost("record")]
    public async Task<IActionResult> RecordPayment(RecordPaymentDto dto)
    {
        try
        {
            var payment = await _service.RecordPaymentAsync(dto);
            return Ok(new { payment.OrderId, payment.UserId, payment.Amount, payment.Status, payment.Timestamp });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to record payment");
            return StatusCode(500, new { error = "Failed to record payment" });
        }
    }

    // GET: api/payments/status/{orderId}
    [HttpGet("status/{orderId}")]
    public async Task<IActionResult> GetPaymentStatus(Guid orderId)
    {
        var payment = await _service.GetPaymentStatusAsync(orderId);
        if (payment == null) return NotFound();
        return Ok(new { payment.OrderId, payment.Status, payment.Timestamp });
    }
}





