namespace ProductService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for product detail views (comprehensive).
/// Contains all product information including related entities.
/// Used for product detail pages where full information is needed.
/// </summary>
public class ProductDetailResponse
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
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last updated timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the organic certification details (if applicable).
    /// Full certification information for detail view.
    /// </summary>
    public CertificationResponse? Certification { get; set; }

    /// <summary>
    /// Gets or sets the product metadata (if applicable).
    /// Extended attributes and SEO information.
    /// </summary>
    public MetadataResponse? Metadata { get; set; }
}
