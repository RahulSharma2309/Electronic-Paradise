namespace OrderService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for order (lightweight).
/// Used for order lists and summaries.
/// Excludes order items for performance.
/// </summary>
public class OrderResponse
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
    /// Gets or sets the item count (for quick display).
    /// </summary>
    public int ItemCount { get; set; }
}
