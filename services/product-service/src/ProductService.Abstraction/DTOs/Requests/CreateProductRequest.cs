using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for creating a new product.
/// Separated from domain model following DTO pattern.
/// Contains validation attributes for API layer.
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// Gets or sets the name of the product. Required, 1-200 characters.
    /// </summary>
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 200 characters")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the product. Optional, max 2000 characters.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the product (in smallest currency unit). Must be non-negative.
    /// Example: 8000 = ₹80.00.
    /// </summary>
    [Required(ErrorMessage = "Price is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Price must be non-negative")]
    public int Price { get; set; }

    /// <summary>
    /// Gets or sets the initial stock quantity. Must be non-negative.
    /// </summary>
    [Required(ErrorMessage = "Stock is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
    public int Stock { get; set; }

    /// <summary>
    /// Gets or sets the product category (e.g., "Fruits", "Vegetables").
    /// </summary>
    [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the brand or manufacturer name.
    /// </summary>
    [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit).
    /// </summary>
    [StringLength(100, ErrorMessage = "SKU cannot exceed 100 characters")]
    public string? Sku { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement (e.g., "kg", "lb", "bunch").
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
    /// Default: true.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the organic certification details (optional).
    /// </summary>
    public CreateCertificationRequest? Certification { get; set; }

    /// <summary>
    /// Gets or sets the product metadata (optional).
    /// </summary>
    public CreateMetadataRequest? Metadata { get; set; }
}
