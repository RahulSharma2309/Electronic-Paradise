using System.ComponentModel.DataAnnotations;

namespace OrderService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for creating an order item.
/// Nested within CreateOrderRequest.
/// </summary>
public class CreateOrderItemRequest
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}
