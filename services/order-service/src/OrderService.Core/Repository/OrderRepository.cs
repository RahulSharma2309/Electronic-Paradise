using Microsoft.EntityFrameworkCore;
using OrderService.Abstraction.Models;
using OrderService.Core.Data;

namespace OrderService.Core.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;
    public OrderRepository(AppDbContext db) => _db = db;

    public async Task<Order> AddAsync(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetByUserIdAsync(Guid userId)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }
}




