namespace OrderService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for order (comprehensive).
/// Used for order detail views with all items.
/// Includes complete order information.
/// </summary>
public class OrderDetailResponse
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID who placed the order.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the total amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the order status.
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the list of order items.
    /// </summary>
    public List<OrderItemResponse> Items { get; set; } = new();
}
