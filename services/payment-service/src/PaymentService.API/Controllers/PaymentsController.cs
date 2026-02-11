using Microsoft.AspNetCore.Mvc;
using PaymentService.Abstraction.DTOs.Requests;
using PaymentService.Abstraction.DTOs.Responses;
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
    /// <param name="request">The payment processing request.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with payment details if successful,
    /// or <see cref="BadRequestObjectResult"/>, <see cref="NotFoundResult"/>,
    /// <see cref="ConflictObjectResult"/>, or <see cref="StatusCodeResult"/> on failure.
    /// </returns>
    [HttpPost("process")]
    [ProducesResponseType(typeof(PaymentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ProcessPayment(ProcessPaymentRequest request)
    {
        _logger.LogInformation("Received payment processing request for Order {OrderId}", request.OrderId);

        try
        {
            var payment = await _service.ProcessPaymentAsync(request);
            _logger.LogInformation("Payment processed successfully for Order {OrderId}, Payment ID: {PaymentId}", request.OrderId, payment.Id);
            return Ok(payment);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Payment processing failed for Order {OrderId}: Invalid argument", request.OrderId);
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Payment processing failed for Order {OrderId}: User not found", request.OrderId);
            return NotFound(new { error = "User not found" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Payment processing failed for Order {OrderId}: {ErrorMessage}", request.OrderId, ex.Message);
            return Conflict(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with User Service for Order {OrderId}", request.OrderId);
            return StatusCode(503, new { error = "User Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Payment processing failed for Order {OrderId}", request.OrderId);
            return StatusCode(500, new { error = "Payment processing failed" });
        }
    }

    /// <summary>
    /// Refunds a payment by crediting the user's wallet.
    /// </summary>
    /// <param name="request">The refund processing request.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with refund details if successful,
    /// or <see cref="BadRequestObjectResult"/> or <see cref="StatusCodeResult"/> on failure.
    /// </returns>
    [HttpPost("refund")]
    [ProducesResponseType(typeof(PaymentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> RefundPayment(RefundPaymentRequest request)
    {
        _logger.LogInformation("Received refund request for Payment {PaymentId}", request.PaymentId);

        try
        {
            var payment = await _service.RefundPaymentAsync(request);
            _logger.LogInformation("Refund processed successfully for Payment {PaymentId}", request.PaymentId);
            return Ok(payment);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Refund processing failed for Payment {PaymentId}: Invalid argument", request.PaymentId);
            return BadRequest(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with User Service for refund of Payment {PaymentId}", request.PaymentId);
            return StatusCode(503, new { error = "User Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Refund processing failed for Payment {PaymentId}", request.PaymentId);
            return StatusCode(500, new { error = "Refund processing failed" });
        }
    }

    // Legacy endpoint removed - use ProcessPayment instead

    /// <summary>
    /// Retrieves the payment status for a specific order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with payment status if found,
    /// or <see cref="NotFoundResult"/> if not found.
    /// </returns>
    [HttpGet("status/{orderId}")]
    [ProducesResponseType(typeof(PaymentStatusResponse), StatusCodes.Status200OK)]
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
            return Ok(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment status for Order {OrderId}", orderId);
            return StatusCode(500, new { error = "Failed to retrieve payment status" });
        }
    }
}
