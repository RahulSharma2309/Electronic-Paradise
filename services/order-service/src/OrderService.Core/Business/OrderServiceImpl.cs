using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OrderService.Abstraction.DTOs;
using OrderService.Abstraction.Models;
using OrderService.Core.Repository;

namespace OrderService.Core.Business;

/// <summary>
/// Provides implementation for order-related business operations with distributed transaction handling.
/// </summary>
public class OrderServiceImpl : IOrderService
{
    private readonly IOrderRepository _repo;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OrderServiceImpl> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderServiceImpl"/> class.
    /// </summary>
    /// <param name="repo">The order repository.</param>
    /// <param name="httpClientFactory">The HTTP client factory for inter-service communication.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if any argument is null.</exception>
    public OrderServiceImpl(
        IOrderRepository repo,
        IHttpClientFactory httpClientFactory,
        ILogger<OrderServiceImpl> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
    {
        _logger.LogInformation("Creating order for user {UserId} with {ItemCount} items", dto.UserId, dto.Items?.Count ?? 0);

        if (dto.Items == null || !dto.Items.Any())
        {
            _logger.LogWarning("Order creation failed for user {UserId}: Order must contain items", dto.UserId);
            throw new ArgumentException("Order must contain items", nameof(dto.Items));
        }

        var productClient = _httpClientFactory.CreateClient("product");
        var paymentClient = _httpClientFactory.CreateClient("payment");
        var userClient = _httpClientFactory.CreateClient("user");

        try
        {
            // 0) Get user profile by userId to get the internal profile Id
            _logger.LogDebug("Fetching user profile for user {UserId}", dto.UserId);
            var userProfileResp = await userClient.GetAsync($"/api/users/by-userid/{dto.UserId}");
            if (userProfileResp.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Order creation failed for user {UserId}: User profile not found", dto.UserId);
                throw new KeyNotFoundException("User profile not found");
            }
            if (!userProfileResp.IsSuccessStatusCode)
            {
                _logger.LogError("User service returned {StatusCode} for user {UserId}", userProfileResp.StatusCode, dto.UserId);
                throw new HttpRequestException($"User service returned {userProfileResp.StatusCode}");
            }

            var userProfile = await userProfileResp.Content.ReadFromJsonAsync<UserProfileDto>();
            if (userProfile == null)
            {
                _logger.LogError("Failed to read user profile for user {UserId}", dto.UserId);
                throw new InvalidOperationException("Failed to read user profile");
            }
            _logger.LogDebug("User profile retrieved successfully: Profile ID {ProfileId}, Wallet Balance: {Balance}", userProfile.Id, userProfile.WalletBalance);

            // 1) Validate stock and collect prices
            _logger.LogDebug("Validating product stock and collecting prices for {ItemCount} items", dto.Items.Count);
            int total = 0;
            var productInfos = new List<(Guid productId, int quantity, int unitPrice)>();
            foreach (var it in dto.Items)
            {
                var res = await productClient.GetAsync($"/api/products/{it.ProductId}");
                if (res.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Order creation failed for user {UserId}: Product {ProductId} not found", dto.UserId, it.ProductId);
                    throw new KeyNotFoundException($"Product not found: {it.ProductId}");
                }
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogError("Product service returned {StatusCode} for product {ProductId}", res.StatusCode, it.ProductId);
                    throw new HttpRequestException($"Product service returned {res.StatusCode}");
                }

                var prod = await res.Content.ReadFromJsonAsync<ProductDto>();
                if (prod == null)
                {
                    _logger.LogError("Failed to read product info for product {ProductId}", it.ProductId);
                    throw new InvalidOperationException("Failed to read product info");
                }
                if (prod.Stock < it.Quantity)
                {
                    _logger.LogWarning("Order creation failed for user {UserId}: Insufficient stock for product {ProductId}. Available: {Stock}, Requested: {Quantity}", dto.UserId, it.ProductId, prod.Stock, it.Quantity);
                    throw new InvalidOperationException($"Insufficient stock for product {it.ProductId}. Available: {prod.Stock}");
                }

                total += prod.Price * it.Quantity;
                productInfos.Add((it.ProductId, it.Quantity, prod.Price));
                _logger.LogDebug("Product {ProductId} validated: Price {Price}, Quantity {Quantity}, Stock {Stock}", it.ProductId, prod.Price, it.Quantity, prod.Stock);
            }
            _logger.LogInformation("Stock validation completed. Total order amount: {TotalAmount}", total);

            // 2) Process payment via Payment Service
            var tempOrderId = Guid.NewGuid();
            _logger.LogInformation("Processing payment for temp order {TempOrderId}, User {UserId}, Amount: {Amount}", tempOrderId, dto.UserId, total);
            var paymentResp = await paymentClient.PostAsJsonAsync("/api/payments/process", new
            {
                OrderId = tempOrderId,
                UserId = dto.UserId,
                UserProfileId = userProfile.Id,
                Amount = total
            });

            if (paymentResp.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Payment processing failed for temp order {TempOrderId}: User not found", tempOrderId);
                throw new KeyNotFoundException("User not found for payment");
            }

            if (paymentResp.StatusCode == HttpStatusCode.Conflict)
            {
                _logger.LogWarning("Payment processing failed for temp order {TempOrderId}: Insufficient balance for user {UserId}", tempOrderId, dto.UserId);
                throw new InvalidOperationException("Payment failed - insufficient balance");
            }

            if (!paymentResp.IsSuccessStatusCode)
            {
                _logger.LogError("Payment service returned {StatusCode} for temp order {TempOrderId}", paymentResp.StatusCode, tempOrderId);
                throw new HttpRequestException($"Payment processing failed with status {paymentResp.StatusCode}");
            }
            _logger.LogInformation("Payment processed successfully for temp order {TempOrderId}", tempOrderId);

            // 3) Reserve stock for each product
            _logger.LogDebug("Reserving stock for {ProductCount} products", productInfos.Count);
            var reserved = new List<(Guid productId, int quantity)>();
            try
            {
                foreach (var p in productInfos)
                {
                    _logger.LogDebug("Reserving {Quantity} units of product {ProductId}", p.quantity, p.productId);
                    var res = await productClient.PostAsJsonAsync($"/api/products/{p.productId}/reserve", new { Quantity = p.quantity });
                    if (res.StatusCode == HttpStatusCode.Conflict || res.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Reservation failed -> refund payment
                        _logger.LogWarning("Stock reservation failed for product {ProductId}. Initiating payment refund for temp order {TempOrderId}", p.productId, tempOrderId);
                        await RefundPaymentAsync(paymentClient, tempOrderId, dto.UserId, userProfile.Id, total);
                        throw new InvalidOperationException($"Stock reservation failed for product {p.productId}");
                    }
                    if (!res.IsSuccessStatusCode)
                    {
                        // Refund payment
                        _logger.LogError("Product service returned {StatusCode} for product {ProductId}. Initiating payment refund for temp order {TempOrderId}", res.StatusCode, p.productId, tempOrderId);
                        await RefundPaymentAsync(paymentClient, tempOrderId, dto.UserId, userProfile.Id, total);
                        throw new HttpRequestException($"Product service returned {res.StatusCode}");
                    }
                    reserved.Add((p.productId, p.quantity));
                    _logger.LogDebug("Stock reserved successfully for product {ProductId}", p.productId);
                }
                _logger.LogInformation("Stock reservation completed for all products");

                // 4) Create order record
                _logger.LogDebug("Creating order record for user {UserId}", dto.UserId);
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
                _logger.LogInformation("Order {OrderId} created successfully for user {UserId}. Total amount: {TotalAmount}, Items: {ItemCount}", order.Id, dto.UserId, total, order.Items.Count);

                return order;
            }
            catch
            {
                // If anything fails after stock reservation, we need to release the reserved stock
                _logger.LogWarning("Order creation failed after stock reservation. Releasing reserved stock for {ReservedCount} products", reserved.Count);
                foreach (var (productId, quantity) in reserved)
                {
                    try
                    {
                        _logger.LogDebug("Releasing {Quantity} units of product {ProductId}", quantity, productId);
                        await productClient.PostAsJsonAsync($"/api/products/{productId}/release", new { Quantity = quantity });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to release stock for product {ProductId}. Manual intervention may be required.", productId);
                    }
                }
                throw;
            }
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException && ex is not HttpRequestException)
        {
            _logger.LogError(ex, "Unexpected error creating order for user {UserId}", dto.UserId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Order?> GetOrderAsync(Guid id)
    {
        _logger.LogDebug("Fetching order {OrderId}", id);
        try
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogDebug("Order {OrderId} not found", id);
            }
            else
            {
                _logger.LogDebug("Order {OrderId} found with {ItemCount} items", id, order.Items?.Count ?? 0);
            }
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching order {OrderId}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
    {
        _logger.LogDebug("Fetching orders for user {UserId}", userId);
        try
        {
            var orders = await _repo.GetByUserIdAsync(userId);
            _logger.LogInformation("Retrieved {OrderCount} orders for user {UserId}", orders.Count, userId);
            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching orders for user {UserId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Refunds a payment by calling the Payment service refund endpoint.
    /// </summary>
    /// <param name="paymentClient">The HTTP client for payment service communication.</param>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="userProfileId">The user profile identifier.</param>
    /// <param name="amount">The amount to refund.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RefundPaymentAsync(HttpClient paymentClient, Guid orderId, Guid userId, Guid userProfileId, int amount)
    {
        _logger.LogInformation("Attempting to refund payment for order {OrderId}, Amount: {Amount}", orderId, amount);
        try
        {
            var refundResp = await paymentClient.PostAsJsonAsync("/api/payments/refund", new
            {
                OrderId = orderId,
                UserId = userId,
                UserProfileId = userProfileId,
                Amount = amount
            });

            if (refundResp.IsSuccessStatusCode)
            {
                _logger.LogInformation("Payment refunded successfully for order {OrderId}", orderId);
            }
            else
            {
                _logger.LogError("Payment refund failed for order {OrderId} with status {StatusCode}", orderId, refundResp.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refund payment for order {OrderId}. Manual refund may be required.", orderId);
        }
    }
}
