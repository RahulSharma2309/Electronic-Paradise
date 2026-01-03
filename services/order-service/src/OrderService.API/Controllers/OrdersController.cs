using Microsoft.AspNetCore.Mvc;
using OrderService.Abstraction.DTOs;
using OrderService.Core.Business;

namespace OrderService.API.Controllers;

/// <summary>
/// Controller for managing orders and orchestrating distributed transactions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly ILogger<OrdersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="service">The order business service.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="service"/> or <paramref name="logger"/> is null.</exception>
    public OrdersController(IOrderService service, ILogger<OrdersController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new order with distributed transaction handling (payment, stock reservation).
    /// </summary>
    /// <param name="dto">The order creation data.</param>
    /// <returns>
    /// A <see cref="CreatedAtActionResult"/> with order details if successful,
    /// or <see cref="BadRequestObjectResult"/>, <see cref="NotFoundObjectResult"/>,
    /// <see cref="ConflictObjectResult"/>, or <see cref="StatusCodeResult"/> on failure.
    /// </returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        _logger.LogInformation("Received order creation request for user {UserId}", dto.UserId);

        try
        {
            var order = await _service.CreateOrderAsync(dto);
            _logger.LogInformation("Order {OrderId} created successfully for user {UserId}. Total: {TotalAmount}", order.Id, order.UserId, order.TotalAmount);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, new
            {
                order.Id,
                order.UserId,
                order.TotalAmount,
                order.CreatedAt
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Order creation failed for user {UserId}: Invalid argument", dto.UserId);
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Order creation failed for user {UserId}: Resource not found", dto.UserId);
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Order creation failed for user {UserId}: {ErrorMessage}", dto.UserId, ex.Message);
            return Conflict(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Service communication failed during order creation for user {UserId}", dto.UserId);
            return StatusCode(503, new { error = "Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Order creation failed for user {UserId}", dto.UserId);
            return StatusCode(500, new { error = "Order creation failed" });
        }
    }

    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with order details if found,
    /// or <see cref="NotFoundResult"/> if not found.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        _logger.LogDebug("Fetching order {OrderId}", id);

        try
        {
            var order = await _service.GetOrderAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Order {OrderId} retrieved successfully", id);
            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order {OrderId}", id);
            return StatusCode(500, new { error = "Failed to retrieve order" });
        }
    }

    /// <summary>
    /// Retrieves all orders for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with a list of orders for the user.
    /// </returns>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        _logger.LogDebug("Fetching orders for user {UserId}", userId);

        try
        {
            var orders = await _service.GetUserOrdersAsync(userId);
            _logger.LogInformation("Retrieved {OrderCount} orders for user {UserId}", orders.Count, userId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving orders for user {UserId}", userId);
            return StatusCode(500, new { error = "Failed to retrieve orders" });
        }
    }
}
