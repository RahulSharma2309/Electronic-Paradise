namespace OrderService.Abstraction.DTOs;

/// <summary>
/// Represents an item within an order creation request.
/// </summary>
public sealed record OrderItemDto
{
    /// <summary>
    /// Gets the unique identifier of the product.
    /// </summary>
    public required Guid ProductId { get; init; }

    /// <summary>
    /// Gets the quantity of the product to order.
    /// </summary>
    public required int Quantity { get; init; }
}
