using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _http;

    public OrdersController(AppDbContext db, IHttpClientFactory http)
    {
        _db = db;
        _http = http;
    }

    // POST: api/orders/create
    // Body: { "userId": "...", "items": [ { "productId": "...", "quantity": 1 } ] }
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        if (dto.Items == null || !dto.Items.Any()) return BadRequest(new { error = "Order must contain items" });

        var productClient = _http.CreateClient("product");
        var paymentClient = _http.CreateClient("payment");
        var userClient = _http.CreateClient("user");

        // 0) Get user profile by userId (Guid) to get the internal profile Id (Guid)
        var userProfileResp = await userClient.GetAsync($"/api/users/by-userid/{dto.UserId}");
        if (userProfileResp.StatusCode == HttpStatusCode.NotFound) 
            return NotFound(new { error = "User profile not found" });
        if (!userProfileResp.IsSuccessStatusCode) 
            return StatusCode((int)userProfileResp.StatusCode);
        
        var userProfile = await userProfileResp.Content.ReadFromJsonAsync<UserProfileDto>();
        if (userProfile == null) 
            return StatusCode(500, new { error = "Failed to read user profile" });

        // 1) Validate stock and collect prices
        int total = 0;
        var productInfos = new List<(Guid productId, int quantity, int unitPrice)>();
        foreach (var it in dto.Items)
        {
            var res = await productClient.GetAsync($"/api/products/{it.ProductId}");
            if (res.StatusCode == HttpStatusCode.NotFound) return NotFound(new { error = "Product not found", productId = it.ProductId });
            if (!res.IsSuccessStatusCode) return StatusCode((int)res.StatusCode);

            var prod = await res.Content.ReadFromJsonAsync<ProductDto>();
            if (prod == null) return StatusCode(500, new { error = "Failed to read product info" });
            if (prod.Stock < it.Quantity) return Conflict(new { error = "Insufficient stock", productId = it.ProductId, available = prod.Stock });

            total += prod.Price * it.Quantity;
            productInfos.Add((it.ProductId, it.Quantity, prod.Price));
        }

        // 2) Process payment via Payment Service (which will debit wallet and record transaction)
        // Create a temporary order ID for payment processing (will be replaced with actual order ID after creation)
        var tempOrderId = Guid.NewGuid();
        var paymentResp = await paymentClient.PostAsJsonAsync("/api/payments/process", new 
        { 
            OrderId = tempOrderId,
            UserId = dto.UserId,
            UserProfileId = userProfile.Id,
            Amount = total 
        });
        
        if (paymentResp.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound(new { error = "User not found" });
        }
        
        if (paymentResp.StatusCode == HttpStatusCode.Conflict)
        {
            var body = await paymentResp.Content.ReadFromJsonAsync<object>();
            return Conflict(new { error = "Payment failed - insufficient balance", details = body });
        }
        
        if (!paymentResp.IsSuccessStatusCode) 
        {
            return StatusCode((int)paymentResp.StatusCode, new { error = "Payment processing failed" });
        }

        // 3) Reserve stock for each product
        var reserved = new List<(Guid productId, int quantity)>();
        foreach (var p in productInfos)
        {
            var res = await productClient.PostAsJsonAsync($"/api/products/{p.productId}/reserve", new { Quantity = p.quantity });
            if (res.StatusCode == HttpStatusCode.Conflict || res.StatusCode == HttpStatusCode.NotFound)
            {
                // Reservation failed -> refund payment via Payment Service
                await paymentClient.PostAsJsonAsync("/api/payments/refund", new 
                { 
                    OrderId = tempOrderId,
                    UserId = dto.UserId,
                    UserProfileId = userProfile.Id,
                    Amount = total 
                });
                var body = await res.Content.ReadFromJsonAsync<object>();
                return Conflict(new { error = "Reservation failed", details = body });
            }
            if (!res.IsSuccessStatusCode)
            {
                // Refund payment via Payment Service
                await paymentClient.PostAsJsonAsync("/api/payments/refund", new 
                { 
                    OrderId = tempOrderId,
                    UserId = dto.UserId,
                    UserProfileId = userProfile.Id,
                    Amount = total 
                });
                return StatusCode((int)res.StatusCode);
            }
            reserved.Add((p.productId, p.quantity));
        }

        // 4) Create order record
        var order = new Order { UserId = dto.UserId, TotalAmount = total };
        foreach (var r in productInfos)
        {
            order.Items.Add(new OrderItem { ProductId = r.productId, Quantity = r.quantity, UnitPrice = r.unitPrice });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, new { order.Id, order.UserId, order.TotalAmount });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    // GET: api/orders/user/{userId}
    // Get all orders for a specific user
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return Ok(orders);
    }
}

public record CreateOrderDto(Guid UserId, List<OrderItemDto> Items);
public record OrderItemDto(Guid ProductId, int Quantity);
public record ProductDto(Guid Id, string Name, string? Description, int Price, int Stock);
public record UserProfileDto(Guid Id, Guid UserId, string? Name, string? Email, string? PhoneNumber, decimal WalletBalance);
