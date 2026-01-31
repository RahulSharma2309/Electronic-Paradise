using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OrderService.Abstraction.DTOs;
using OrderService.Abstraction.DTOs.Requests;
using OrderService.Abstraction.DTOs.Responses;
using OrderService.Abstraction.Models;
using OrderService.Core.Mappers;
using OrderService.Core.Repository;

namespace OrderService.Core.Business;

/// <summary>
/// Provides implementation for order-related business operations with distributed transaction handling.
/// </summary>
public class OrderServiceImpl : IOrderService
{
    private readonly IOrderRepository _repo;
    private readonly IOrderMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OrderServiceImpl> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderServiceImpl"/> class.
    /// </summary>
    /// <param name="repo">The order repository.</param>
    /// <param name="mapper">The order mapper.</param>
    /// <param name="httpClientFactory">The HTTP client factory for inter-service communication.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if any argument is null.</exception>
    public OrderServiceImpl(
        IOrderRepository repo,
        IOrderMapper mapper,
        IHttpClientFactory httpClientFactory,
        ILogger<OrderServiceImpl> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<OrderDetailResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        _logger.LogInformation("Creating order for user {UserId} with {ItemCount} items", request.UserId, request.Items?.Count ?? 0);

        if (request.Items == null || !request.Items.Any())
        {
            _logger.LogWarning("Order creation failed for user {UserId}: Order must contain items", request.UserId);
            throw new ArgumentException("Order must contain items", nameof(request.Items));
        }

        var productClient = _httpClientFactory.CreateClient("product");
        var paymentClient = _httpClientFactory.CreateClient("payment");
        var userClient = _httpClientFactory.CreateClient("user");

        try
        {
            // 0) Get user profile by userId to get the internal profile Id
            _logger.LogDebug("Fetching user profile for user {UserId}", request.UserId);
            var userProfileResp = await userClient.GetAsync($"/api/users/by-userid/{request.UserId}");
            if (userProfileResp.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Order creation failed for user {UserId}: User profile not found", request.UserId);
                throw new KeyNotFoundException("User profile not found");
            }

            if (!userProfileResp.IsSuccessStatusCode)
            {
                _logger.LogError("User service returned {StatusCode} for user {UserId}", userProfileResp.StatusCode, request.UserId);
                throw new HttpRequestException($"User service returned {userProfileResp.StatusCode}");
            }

            var userProfile = await userProfileResp.Content.ReadFromJsonAsync<UserProfileDto>();
            if (userProfile == null)
            {
                _logger.LogError("Failed to read user profile for user {UserId}", request.UserId);
                throw new InvalidOperationException("Failed to read user profile");
            }

            _logger.LogDebug("User profile retrieved successfully: Profile ID {ProfileId}, Wallet Balance: {Balance}", userProfile.Id, userProfile.WalletBalance);

            // 1) Validate stock and collect prices
            _logger.LogDebug("Validating product stock and collecting prices for {ItemCount} items", request.Items.Count);
            int total = 0;
            var productInfos = new List<(Guid ProductId, int Quantity, int UnitPrice)>();
            foreach (var it in request.Items)
            {
                var res = await productClient.GetAsync($"/api/products/{it.ProductId}");
                if (res.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Order creation failed for user {UserId}: Product {ProductId} not found", request.UserId, it.ProductId);
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
                    _logger.LogWarning("Order creation failed for user {UserId}: Insufficient stock for product {ProductId}. Available: {Stock}, Requested: {Quantity}", request.UserId, it.ProductId, prod.Stock, it.Quantity);
                    throw new InvalidOperationException($"Insufficient stock for product {it.ProductId}. Available: {prod.Stock}");
                }

                total += prod.Price * it.Quantity;
                productInfos.Add((it.ProductId, it.Quantity, prod.Price));
                _logger.LogDebug("Product {ProductId} validated: Price {Price}, Quantity {Quantity}, Stock {Stock}", it.ProductId, prod.Price, it.Quantity, prod.Stock);
            }

            _logger.LogInformation("Stock validation completed. Total order amount: {TotalAmount}", total);

            // 2) Process payment via Payment Service
            var tempOrderId = Guid.NewGuid();
            _logger.LogInformation("Processing payment for temp order {TempOrderId}, User {UserId}, Amount: {Amount}", tempOrderId, request.UserId, total);
            var paymentResp = await paymentClient.PostAsJsonAsync("/api/payments/process", new
            {
                OrderId = tempOrderId,
                UserId = request.UserId,
                Amount = total,
            });

            if (paymentResp.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Payment processing failed for temp order {TempOrderId}: User not found", tempOrderId);
                throw new KeyNotFoundException("User not found for payment");
            }

            if (paymentResp.StatusCode == HttpStatusCode.Conflict)
            {
                _logger.LogWarning("Payment processing failed for temp order {TempOrderId}: Insufficient balance for user {UserId}", tempOrderId, request.UserId);
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
            var reserved = new List<(Guid ProductId, int Quantity)>();
            try
            {
                foreach (var p in productInfos)
                {
                    _logger.LogDebug("Reserving {Quantity} units of product {ProductId}", p.Quantity, p.ProductId);
                    var res = await productClient.PostAsJsonAsync($"/api/products/{p.ProductId}/reserve", new { Quantity = p.Quantity });
                    if (res.StatusCode == HttpStatusCode.Conflict || res.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Reservation failed -> refund payment
                        _logger.LogWarning("Stock reservation failed for product {ProductId}. Initiating payment refund for temp order {TempOrderId}", p.ProductId, tempOrderId);
                        await RefundPaymentAsync(paymentClient, tempOrderId);
                        throw new InvalidOperationException($"Stock reservation failed for product {p.ProductId}");
                    }

                    if (!res.IsSuccessStatusCode)
                    {
                        // Refund payment
                        _logger.LogError("Product service returned {StatusCode} for product {ProductId}. Initiating payment refund for temp order {TempOrderId}", res.StatusCode, p.ProductId, tempOrderId);
                        await RefundPaymentAsync(paymentClient, tempOrderId);
                        throw new HttpRequestException($"Product service returned {res.StatusCode}");
                    }

                    reserved.Add((p.ProductId, p.Quantity));
                    _logger.LogDebug("Stock reserved successfully for product {ProductId}", p.ProductId);
                }

                _logger.LogInformation("Stock reservation completed for all products");

                // 4) Create order record
                _logger.LogDebug("Creating order record for user {UserId}", request.UserId);
                var order = _mapper.ToEntity(request);
                order.TotalAmount = total;

                // Map items with unit prices from product validation
                order.Items.Clear();
                foreach (var r in productInfos)
                {
                    order.Items.Add(new OrderItem
                    {
                        ProductId = r.ProductId,
                        Quantity = r.Quantity,
                        UnitPrice = r.UnitPrice,
                    });
                }

                await _repo.AddAsync(order);
                _logger.LogInformation("Order {OrderId} created successfully for user {UserId}. Total amount: {TotalAmount}, Items: {ItemCount}", order.Id, request.UserId, total, order.Items.Count);

                return _mapper.ToDetailResponse(order);
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
            _logger.LogError(ex, "Unexpected error creating order for user {UserId}", request.UserId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<OrderDetailResponse?> GetOrderAsync(Guid id)
    {
        _logger.LogDebug("Fetching order {OrderId}", id);
        try
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogDebug("Order {OrderId} not found", id);
                return null;
            }

            _logger.LogDebug("Order {OrderId} found with {ItemCount} items", id, order.Items?.Count ?? 0);
            return _mapper.ToDetailResponse(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching order {OrderId}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<OrderResponse>> GetUserOrdersAsync(Guid userId)
    {
        _logger.LogDebug("Fetching orders for user {UserId}", userId);
        try
        {
            var orders = await _repo.GetByUserIdAsync(userId);
            _logger.LogInformation("Retrieved {OrderCount} orders for user {UserId}", orders.Count, userId);
            return orders.Select(_mapper.ToResponse).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching orders for user {UserId}", userId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<OrderDetailResponse?> UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        _logger.LogInformation("Updating order {OrderId}", id);
        try
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Update failed: Order {OrderId} not found", id);
                return null;
            }

            // Update order properties - Order model doesn't have Status yet, so just update notes if needed
            // In the future, add Status field to Order model and update it here
            var updated = await _repo.UpdateAsync(order);

            _logger.LogInformation("Order {OrderId} updated successfully", id);
            return _mapper.ToDetailResponse(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order {OrderId}", id);
            throw;
        }
    }

    /// <summary>
    /// Refunds a payment by calling the Payment service refund endpoint.
    /// </summary>
    /// <param name="paymentClient">The HTTP client for payment service communication.</param>
    /// <param name="orderId">The order identifier.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RefundPaymentAsync(HttpClient paymentClient, Guid orderId)
    {
        _logger.LogInformation("Attempting to refund payment for order {OrderId}", orderId);
        try
        {
            // Get payment by order ID first
            var paymentResp = await paymentClient.GetAsync($"/api/payments/status/{orderId}");
            if (paymentResp.IsSuccessStatusCode)
            {
                var paymentStatus = await paymentResp.Content.ReadFromJsonAsync<PaymentStatusResponse>();
                if (paymentStatus != null)
                {
                    var refundResp = await paymentClient.PostAsJsonAsync("/api/payments/refund", new
                    {
                        PaymentId = paymentStatus.PaymentId,
                        Reason = "Order creation failed - auto refund",
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
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refund payment for order {OrderId}. Manual refund may be required.", orderId);
        }
    }

    // Helper response DTO for payment status
    private class PaymentStatusResponse
    {
        public Guid PaymentId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
