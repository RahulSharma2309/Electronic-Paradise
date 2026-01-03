using OrderService.Abstraction.DTOs;
using OrderService.Abstraction.Models;

namespace OrderService.Core.Business;

/// <summary>
/// Defines the contract for order-related business operations.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates a new order by validating products, processing payment, and reserving stock.
    /// </summary>
    /// <param name="dto">The order creation data.</param>
    /// <returns>The created order.</returns>
    /// <exception cref="ArgumentException">Thrown if the order contains no items.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the user profile or product is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if there is insufficient stock or balance.</exception>
    /// <exception cref="HttpRequestException">Thrown if external service communication fails.</exception>
    Task<Order> CreateOrderAsync(CreateOrderDto dto);

    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The order if found, otherwise null.</returns>
    Task<Order?> GetOrderAsync(Guid id);

    /// <summary>
    /// Retrieves all orders for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of orders for the user.</returns>
    Task<List<Order>> GetUserOrdersAsync(Guid userId);
}
