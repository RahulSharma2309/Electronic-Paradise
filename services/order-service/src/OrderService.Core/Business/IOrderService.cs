using OrderService.Abstraction.DTOs.Requests;
using OrderService.Abstraction.DTOs.Responses;

namespace OrderService.Core.Business;

/// <summary>
/// Defines the contract for order-related business operations.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates a new order by validating products, processing payment, and reserving stock.
    /// </summary>
    /// <param name="request">The order creation request.</param>
    /// <returns>The created order with comprehensive details.</returns>
    /// <exception cref="ArgumentException">Thrown if the order contains no items.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the user profile or product is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if there is insufficient stock or balance.</exception>
    /// <exception cref="HttpRequestException">Thrown if external service communication fails.</exception>
    Task<OrderDetailResponse> CreateOrderAsync(CreateOrderRequest request);

    /// <summary>
    /// Retrieves an order by its unique identifier (comprehensive details).
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The order if found, otherwise null.</returns>
    Task<OrderDetailResponse?> GetOrderAsync(Guid id);

    /// <summary>
    /// Retrieves all orders for a specific user (lightweight for lists).
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of orders for the user.</returns>
    Task<List<OrderResponse>> GetUserOrdersAsync(Guid userId);

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="request">The order update request.</param>
    /// <returns>The updated order if found, otherwise null.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the order is not found.</exception>
    Task<OrderDetailResponse?> UpdateOrderAsync(Guid id, UpdateOrderRequest request);
}
