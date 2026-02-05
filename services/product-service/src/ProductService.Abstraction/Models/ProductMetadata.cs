using System.Text.Json;

namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents flexible metadata for a product.
/// Enables white-label extensibility without modifying core Product schema.
/// Follows Open/Closed Principle - open for extension, closed for modification.
/// </summary>
public class ProductMetadata
{
    /// <summary>
    /// Gets or sets the unique identifier for the metadata record.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the product ID this metadata belongs to.
    /// Foreign key to Product entity.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets SEO-friendly slug for the product.
    /// Example: "organic-bananas-kerala".
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets SEO metadata (title, description, keywords).
    /// Stored as JSON for flexibility.
    /// </summary>
    public string? SeoMetadataJson { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this metadata was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last updated timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // ============================================================================
    // Navigation Properties
    // ============================================================================

    /// <summary>
    /// Gets or sets the product this metadata belongs to.
    /// Navigation property for EF Core.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Deserializes the <see cref="SeoMetadataJson"/> property.
    /// </summary>
    /// <returns>
    /// The deserialized <see cref="SeoMetadata"/> if <see cref="SeoMetadataJson"/> contains valid JSON; otherwise, <see langword="null" />.
    /// </returns>
    public SeoMetadata? GetSeoMetadata()
    {
        if (string.IsNullOrWhiteSpace(this.SeoMetadataJson))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<SeoMetadata>(this.SeoMetadataJson);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Serializes SEO metadata to JSON.
    /// </summary>
    /// <param name="seoMetadata">The SEO metadata to serialize.</param>
    public void SetSeoMetadata(SeoMetadata? seoMetadata)
    {
        if (seoMetadata == null)
        {
            this.SeoMetadataJson = null;
            return;
        }

        this.SeoMetadataJson = JsonSerializer.Serialize(seoMetadata);
    }
}
