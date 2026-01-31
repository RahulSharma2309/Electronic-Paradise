using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for reserving product stock.
/// Used by OrderService to hold inventory during checkout.
/// </summary>
public class ReserveStockRequest
{
    /// <summary>
    /// Gets or sets the quantity to reserve.
    /// </summary>
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}
