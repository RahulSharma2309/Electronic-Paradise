using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Abstraction.Models;
using OrderService.Core.Data;

namespace OrderService.Core.Repository;

/// <summary>
/// Provides data access operations for <see cref="Order"/> entities.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<OrderRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="db">The application database context.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="db"/> or <paramref name="logger"/> is null.</exception>
    public OrderRepository(AppDbContext db, ILogger<OrderRepository> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<Order> AddAsync(Order order)
    {
        _logger.LogDebug("Adding order for user {UserId} with {ItemCount} items", order.UserId, order.Items?.Count ?? 0);
        try
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} added successfully for user {UserId}", order.Id, order.UserId);
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add order for user {UserId}", order.UserId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("Fetching order {OrderId} with items", id);
        try
        {
            var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
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
    public async Task<List<Order>> GetByUserIdAsync(Guid userId)
    {
        _logger.LogDebug("Fetching orders for user {UserId}", userId);
        try
        {
            var orders = await _db.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            _logger.LogDebug("Retrieved {OrderCount} orders for user {UserId}", orders.Count, userId);
            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching orders for user {UserId}", userId);
            throw;
        }
    }
}
