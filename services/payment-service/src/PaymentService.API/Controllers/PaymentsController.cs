using Microsoft.AspNetCore.Mvc;
using PaymentService.Abstraction.DTOs;
using PaymentService.Core.Business;

namespace PaymentService.API.Controllers;

/// <summary>
/// Controller for managing payment operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _service;
    private readonly ILogger<PaymentsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentsController"/> class.
    /// </summary>
    /// <param name="service">The payment business service.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="service"/> or <paramref name="logger"/> is null.</exception>
    public PaymentsController(IPaymentService service, ILogger<PaymentsController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Processes a payment by debiting the user's wallet.
    /// </summary>
    /// <param name="dto">The payment processing data.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with payment details if successful,
    /// or <see cref="BadRequestObjectResult"/>, <see cref="NotFoundResult"/>,
    /// <see cref="ConflictObjectResult"/>, or <see cref="StatusCodeResult"/> on failure.
    /// </returns>
    [HttpPost("process")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ProcessPayment(ProcessPaymentDto dto)
    {
        _logger.LogInformation("Received payment processing request for Order {OrderId}", dto.OrderId);

        try
        {
            var payment = await _service.ProcessPaymentAsync(dto);
            _logger.LogInformation("Payment processed successfully for Order {OrderId}, Payment ID: {PaymentId}", dto.OrderId, payment.Id);
            return Ok(new
            {
                paymentId = payment.Id,
                orderId = payment.OrderId,
                userId = payment.UserId,
                amount = payment.Amount,
                status = payment.Status,
                timestamp = payment.Timestamp,
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Payment processing failed for Order {OrderId}: Invalid argument", dto.OrderId);
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Payment processing failed for Order {OrderId}: User not found", dto.OrderId);
            return NotFound(new { error = "User not found" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Payment processing failed for Order {OrderId}: {ErrorMessage}", dto.OrderId, ex.Message);
            return Conflict(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with User Service for Order {OrderId}", dto.OrderId);
            return StatusCode(503, new { error = "User Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Payment processing failed for Order {OrderId}", dto.OrderId);
            return StatusCode(500, new { error = "Payment processing failed" });
        }
    }

    /// <summary>
    /// Refunds a payment by crediting the user's wallet.
    /// </summary>
    /// <param name="dto">The refund processing data.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with refund details if successful,
    /// or <see cref="BadRequestObjectResult"/> or <see cref="StatusCodeResult"/> on failure.
    /// </returns>
    [HttpPost("refund")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> RefundPayment(RefundPaymentDto dto)
    {
        _logger.LogInformation("Received refund request for Order {OrderId}", dto.OrderId);

        try
        {
            var payment = await _service.RefundPaymentAsync(dto);
            _logger.LogInformation("Refund processed successfully for Order {OrderId}, Payment ID: {PaymentId}", dto.OrderId, payment.Id);
            return Ok(new
            {
                paymentId = payment.Id,
                orderId = payment.OrderId,
                userId = payment.UserId,
                amount = payment.Amount,
                status = payment.Status,
                timestamp = payment.Timestamp,
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Refund processing failed for Order {OrderId}: Invalid argument", dto.OrderId);
            return BadRequest(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with User Service for refund of Order {OrderId}", dto.OrderId);
            return StatusCode(503, new { error = "User Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Refund processing failed for Order {OrderId}", dto.OrderId);
            return StatusCode(500, new { error = "Refund processing failed" });
        }
    }

    /// <summary>
    /// Records a payment transaction (legacy endpoint for compatibility).
    /// </summary>
    /// <param name="dto">The payment record data.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with payment details if successful,
    /// or <see cref="StatusCodeResult"/> on failure.
    /// </returns>
    [HttpPost("record")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RecordPayment(RecordPaymentDto dto)
    {
        _logger.LogInformation("Received payment record request for Order {OrderId}", dto.OrderId);

        try
        {
            var payment = await _service.RecordPaymentAsync(dto);
            _logger.LogInformation("Payment recorded successfully for Order {OrderId}, Payment ID: {PaymentId}", dto.OrderId, payment.Id);
            return Ok(new { payment.OrderId, payment.UserId, payment.Amount, payment.Status, payment.Timestamp });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to record payment for Order {OrderId}", dto.OrderId);
            return StatusCode(500, new { error = "Failed to record payment" });
        }
    }

    /// <summary>
    /// Retrieves the payment status for a specific order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with payment status if found,
    /// or <see cref="NotFoundResult"/> if not found.
    /// </returns>
    [HttpGet("status/{orderId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPaymentStatus(Guid orderId)
    {
        _logger.LogDebug("Fetching payment status for Order {OrderId}", orderId);

        try
        {
            var payment = await _service.GetPaymentStatusAsync(orderId);
            if (payment == null)
            {
                _logger.LogWarning("Payment status not found for Order {OrderId}", orderId);
                return NotFound();
            }

            _logger.LogInformation("Payment status retrieved successfully for Order {OrderId}: {Status}", orderId, payment.Status);
            return Ok(new { payment.OrderId, payment.Status, payment.Timestamp });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment status for Order {OrderId}", orderId);
            return StatusCode(500, new { error = "Failed to retrieve payment status" });
        }
    }
}
