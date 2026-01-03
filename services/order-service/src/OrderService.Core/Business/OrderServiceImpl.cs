using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OrderService.Abstraction.DTOs;
using OrderService.Abstraction.Models;
using OrderService.Core.Repository;

namespace OrderService.Core.Business;

public class OrderServiceImpl : IOrderService
{
    private readonly IOrderRepository _repo;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OrderServiceImpl> _logger;

    public OrderServiceImpl(
        IOrderRepository repo,
        IHttpClientFactory httpClientFactory,
        ILogger<OrderServiceImpl> logger)
    {
        _repo = repo;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new ArgumentException("Order must contain items");

        var productClient = _httpClientFactory.CreateClient("product");
        var paymentClient = _httpClientFactory.CreateClient("payment");
        var userClient = _httpClientFactory.CreateClient("user");

        // 0) Get user profile by userId to get the internal profile Id
        var userProfileResp = await userClient.GetAsync($"/api/users/by-userid/{dto.UserId}");
        if (userProfileResp.StatusCode == HttpStatusCode.NotFound)
            throw new KeyNotFoundException("User profile not found");
        if (!userProfileResp.IsSuccessStatusCode)
            throw new HttpRequestException($"User service returned {userProfileResp.StatusCode}");

        var userProfile = await userProfileResp.Content.ReadFromJsonAsync<UserProfileDto>();
        if (userProfile == null)
            throw new InvalidOperationException("Failed to read user profile");

        // 1) Validate stock and collect prices
        int total = 0;
        var productInfos = new List<(Guid productId, int quantity, int unitPrice)>();
        foreach (var it in dto.Items)
        {
            var res = await productClient.GetAsync($"/api/products/{it.ProductId}");
            if (res.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException($"Product not found: {it.ProductId}");
            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"Product service returned {res.StatusCode}");

            var prod = await res.Content.ReadFromJsonAsync<ProductDto>();
            if (prod == null)
                throw new InvalidOperationException("Failed to read product info");
            if (prod.Stock < it.Quantity)
                throw new InvalidOperationException($"Insufficient stock for product {it.ProductId}. Available: {prod.Stock}");

            total += prod.Price * it.Quantity;
            productInfos.Add((it.ProductId, it.Quantity, prod.Price));
        }

        // 2) Process payment via Payment Service
        var tempOrderId = Guid.NewGuid();
        var paymentResp = await paymentClient.PostAsJsonAsync("/api/payments/process", new
        {
            OrderId = tempOrderId,
            UserId = dto.UserId,
            UserProfileId = userProfile.Id,
            Amount = total
        });

        if (paymentResp.StatusCode == HttpStatusCode.NotFound)
            throw new KeyNotFoundException("User not found for payment");

        if (paymentResp.StatusCode == HttpStatusCode.Conflict)
            throw new InvalidOperationException("Payment failed - insufficient balance");

        if (!paymentResp.IsSuccessStatusCode)
            throw new HttpRequestException($"Payment processing failed with status {paymentResp.StatusCode}");

        // 3) Reserve stock for each product
        var reserved = new List<(Guid productId, int quantity)>();
        try
        {
            foreach (var p in productInfos)
            {
                var res = await productClient.PostAsJsonAsync($"/api/products/{p.productId}/reserve", new { Quantity = p.quantity });
                if (res.StatusCode == HttpStatusCode.Conflict || res.StatusCode == HttpStatusCode.NotFound)
                {
                    // Reservation failed -> refund payment
                    await RefundPaymentAsync(paymentClient, tempOrderId, dto.UserId, userProfile.Id, total);
                    throw new InvalidOperationException($"Stock reservation failed for product {p.productId}");
                }
                if (!res.IsSuccessStatusCode)
                {
                    // Refund payment
                    await RefundPaymentAsync(paymentClient, tempOrderId, dto.UserId, userProfile.Id, total);
                    throw new HttpRequestException($"Product service returned {res.StatusCode}");
                }
                reserved.Add((p.productId, p.quantity));
            }

            // 4) Create order record
            var order = new Order { UserId = dto.UserId, TotalAmount = total };
            foreach (var r in productInfos)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = r.productId,
                    Quantity = r.quantity,
                    UnitPrice = r.unitPrice
                });
            }

            await _repo.AddAsync(order);
            _logger.LogInformation("Order {OrderId} created successfully for user {UserId}", order.Id, dto.UserId);

            return order;
        }
        catch
        {
            // If anything fails after stock reservation, we need to release the reserved stock
            foreach (var (productId, quantity) in reserved)
            {
                try
                {
                    await productClient.PostAsJsonAsync($"/api/products/{productId}/release", new { Quantity = quantity });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to release stock for product {ProductId}", productId);
                }
            }
            throw;
        }
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
    {
        return await _repo.GetByUserIdAsync(userId);
    }

    private async Task RefundPaymentAsync(HttpClient paymentClient, Guid orderId, Guid userId, Guid userProfileId, int amount)
    {
        try
        {
            await paymentClient.PostAsJsonAsync("/api/payments/refund", new
            {
                OrderId = orderId,
                UserId = userId,
                UserProfileId = userProfileId,
                Amount = amount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refund payment for order {OrderId}", orderId);
        }
    }
}





