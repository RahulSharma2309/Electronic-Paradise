namespace OrderService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for creating a new order.
/// </summary>
public sealed record CreateOrderDto
{
    /// <summary>
    /// Gets the unique identifier of the user placing the order.
    /// </summary>
    required public Guid UserId { get; init; }

    /// <summary>
    /// Gets the list of items to include in the order.
    /// </summary>
    required public List<OrderItemDto> Items { get; init; }
}
