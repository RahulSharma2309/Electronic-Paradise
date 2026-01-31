namespace OrderService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for order item.
/// Nested within OrderDetailResponse.
/// </summary>
public class OrderItemResponse
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the order ID.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price at time of order.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets the total price for this item (Quantity * UnitPrice).
    /// </summary>
    public decimal TotalPrice => Quantity * UnitPrice;
}
