namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents an image/media record for a product.
/// In real scenarios the binary file lives in object storage (S3/Azure Blob/etc.),
/// and the database stores only metadata + URL/keys.
/// </summary>
public class ProductImage
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the product ID this image belongs to.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the image URL (or CDN URL).
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// Gets or sets optional alt text for accessibility.
    /// </summary>
    public string? AltText { get; set; }

    /// <summary>
    /// Gets or sets an ordering number for image galleries.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is the primary image.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the product navigation.
    /// </summary>
    public Product Product { get; set; } = null!;
}
