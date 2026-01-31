using OrderService.Abstraction.DTOs.Requests;
using OrderService.Abstraction.DTOs.Responses;
using OrderService.Abstraction.Models;

namespace OrderService.Core.Mappers;

/// <summary>
/// Implementation of order mapping between domain models and DTOs.
/// </summary>
public class OrderMapper : IOrderMapper
{
    /// <inheritdoc/>
    public OrderResponse ToResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = "Completed", // Default status since Order model doesn't have status yet
            CreatedAt = order.CreatedAt,
            ItemCount = order.Items?.Count ?? 0,
        };
    }

    /// <inheritdoc/>
    public OrderDetailResponse ToDetailResponse(Order order)
    {
        return new OrderDetailResponse
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = "Completed", // Default status since Order model doesn't have status yet
            CreatedAt = order.CreatedAt,
            Items = order.Items?.Select(item => new OrderItemResponse
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
            }).ToList() ?? new List<OrderItemResponse>(),
        };
    }

    /// <inheritdoc/>
    public Order ToEntity(CreateOrderRequest request)
    {
        return new Order
        {
            UserId = request.UserId,
            TotalAmount = 0, // Will be calculated based on items
            CreatedAt = DateTime.UtcNow,
            Items = request.Items?.Select(itemRequest => new OrderItem
            {
                ProductId = itemRequest.ProductId,
                Quantity = itemRequest.Quantity,
                UnitPrice = 0, // Will be set from product data
            }).ToList() ?? new List<OrderItem>(),
        };
    }
}
