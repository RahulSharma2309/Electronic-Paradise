using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for updating an existing product.
/// All fields are optional - only provided fields will be updated (PATCH semantics).
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 200 characters")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the product (in smallest currency unit).
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Price must be non-negative")]
    public int? Price { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
    public int? Stock { get; set; }

    /// <summary>
    /// Gets or sets the product category.
    /// </summary>
    [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the brand or manufacturer name.
    /// </summary>
    [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the SKU.
    /// </summary>
    [StringLength(100, ErrorMessage = "SKU cannot exceed 100 characters")]
    public string? Sku { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement.
    /// </summary>
    [StringLength(50, ErrorMessage = "Unit cannot exceed 50 characters")]
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
    [Url(ErrorMessage = "Image URL must be a valid URL")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is active/visible.
    /// </summary>
    public bool? IsActive { get; set; }
}
