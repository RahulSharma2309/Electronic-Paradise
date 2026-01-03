using Microsoft.AspNetCore.Mvc;
using OrderService.Abstraction.DTOs;
using OrderService.Core.Business;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService service, ILogger<OrdersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // POST: api/orders/create
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        try
        {
            var order = await _service.CreateOrderAsync(dto);
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
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Service communication failed");
            return StatusCode(503, new { error = "Service unavailable" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Order creation failed");
            return StatusCode(500, new { error = "Order creation failed" });
        }
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _service.GetOrderAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    // GET: api/orders/user/{userId}
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        var orders = await _service.GetUserOrdersAsync(userId);
        return Ok(orders);
    }
}





