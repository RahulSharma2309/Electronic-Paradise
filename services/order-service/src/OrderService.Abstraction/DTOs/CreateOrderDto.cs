namespace OrderService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for creating a new order.
/// </summary>
public sealed record CreateOrderDto
{
    /// <summary>
    /// Gets the unique identifier of the user placing the order.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Gets the list of items to include in the order.
    /// </summary>
    public required List<OrderItemDto> Items { get; init; }
}
