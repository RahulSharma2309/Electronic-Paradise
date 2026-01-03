using OrderService.Abstraction.Models;

namespace OrderService.Core.Repository;

/// <summary>
/// Defines the contract for order data access operations.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Adds a new order to the repository.
    /// </summary>
    /// <param name="order">The order to add.</param>
    /// <returns>The added order with generated ID.</returns>
    Task<Order> AddAsync(Order order);

    /// <summary>
    /// Retrieves an order by its unique identifier, including its items.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The order with items if found, otherwise null.</returns>
    Task<Order?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all orders for a specific user, ordered by creation date descending.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of orders with items for the user.</returns>
    Task<List<Order>> GetByUserIdAsync(Guid userId);
}
