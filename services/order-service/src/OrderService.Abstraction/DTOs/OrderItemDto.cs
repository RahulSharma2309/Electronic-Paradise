namespace OrderService.Abstraction.DTOs;

/// <summary>
/// Represents an item within an order creation request.
/// </summary>
public sealed record OrderItemDto
{
    /// <summary>
    /// Gets the unique identifier of the product.
    /// </summary>
    required public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the quantity of the product to order.
    /// </summary>
    required public int Quantity { get; init; }
}
