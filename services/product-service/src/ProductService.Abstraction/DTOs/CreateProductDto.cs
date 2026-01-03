using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for creating a new product.
/// </summary>
public class CreateProductDto
{
    /// <summary>
    /// Gets or sets the name of the product. Required, 1-200 characters.
    /// </summary>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the product. Optional, max 2000 characters.
    /// </summary>
    [StringLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the product. Must be non-negative.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Price { get; set; }

    /// <summary>
    /// Gets or sets the initial stock quantity. Must be non-negative.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
