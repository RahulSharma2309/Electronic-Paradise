namespace ProductService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for product list views (lightweight).
/// Contains only essential fields for product listings.
/// Excludes heavy nested objects for performance.
/// </summary>
public class ProductResponse
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price (in smallest currency unit).
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// Gets or sets the available stock quantity.
    /// </summary>
    public int Stock { get; set; }

    /// <summary>
    /// Gets or sets the product category.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the brand.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the SKU.
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement.
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product has organic certification.
    /// Quick flag for UI badges without loading full certification details.
    /// </summary>
    public bool HasCertification { get; set; }

    /// <summary>
    /// Gets or sets the certification type (if applicable).
    /// Quick display for product cards.
    /// </summary>
    public string? CertificationType { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
