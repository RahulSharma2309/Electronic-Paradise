using System.ComponentModel.DataAnnotations;

namespace OrderService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for creating a new order.
/// Contains validation attributes for API layer.
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Gets or sets the user ID placing the order.
    /// </summary>
    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the list of order items.
    /// </summary>
    [Required(ErrorMessage = "Order items are required")]
    [MinLength(1, ErrorMessage = "Order must contain at least one item")]
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}
