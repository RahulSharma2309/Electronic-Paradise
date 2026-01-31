using System.ComponentModel.DataAnnotations;

namespace OrderService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for updating an order.
/// Used for order modifications (e.g., status updates, cancellations).
/// </summary>
public class UpdateOrderRequest
{
    /// <summary>
    /// Gets or sets the order status.
    /// Values: "Pending", "Processing", "Shipped", "Delivered", "Cancelled".
    /// </summary>
    [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets optional notes for the update.
    /// </summary>
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}
