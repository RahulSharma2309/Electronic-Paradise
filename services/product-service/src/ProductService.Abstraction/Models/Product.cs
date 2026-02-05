namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents a product in the Product service.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// Gets or sets the available stock quantity.
    /// </summary>
    public int Stock { get; set; }

    /// <summary>
    /// Gets or sets the category ID.
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the category navigation.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// Gets or sets the brand or manufacturer name.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit).
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement (e.g., "kg", "lb", "bunch").
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets the product images (gallery + primary).
    /// </summary>
    public List<ProductImage> Images { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the product is active/visible.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the timestamp when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp when the product was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the optional organic certification details for the product.
    /// </summary>
    public ProductCertification? Certification { get; set; }

    /// <summary>
    /// Gets or sets the optional extended metadata for the product.
    /// </summary>
    public ProductMetadata? Metadata { get; set; }

    /// <summary>
    /// Gets or sets queryable attributes/specifications for the product.
    /// </summary>
    public List<ProductAttribute> Attributes { get; set; } = new();

    /// <summary>
    /// Gets or sets tags for the product (many-to-many).
    /// </summary>
    public List<ProductTag> ProductTags { get; set; } = new();
}
