using OrderService.Abstraction.DTOs;
using OrderService.Abstraction.Models;

namespace OrderService.Core.Business;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderDto dto);
    Task<Order?> GetOrderAsync(Guid id);
    Task<List<Order>> GetUserOrdersAsync(Guid userId);
}




