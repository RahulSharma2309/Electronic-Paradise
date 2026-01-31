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
    /// Gets or sets flexible product attributes as a JSON string.
    /// Allows for custom fields without schema changes (white-label ready).
    /// Example: {"organic": true, "gmo_free": true, "allergens": ["nuts"], "vegan": false}.
    /// </summary>
    public string? AttributesJson { get; set; }

    /// <summary>
    /// Gets or sets custom tags for search and categorization.
    /// Example: ["organic", "local", "seasonal", "fresh"].
    /// </summary>
    public string[]? Tags { get; set; }

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

    // ============================================================================
    // Helper Methods
    // ============================================================================

    /// <summary>
    /// Deserializes the AttributesJson property to a dictionary.
    /// Returns null if AttributesJson is null or invalid.
    /// </summary>
    /// <returns>
    /// A dictionary of attributes if <see cref="AttributesJson"/> contains valid JSON; otherwise, <see langword="null" />.
    /// </returns>
    public Dictionary<string, object>? GetAttributes()
    {
        if (string.IsNullOrWhiteSpace(AttributesJson))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(AttributesJson);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Serializes a dictionary to JSON and sets the AttributesJson property.
    /// </summary>
    /// <param name="attributes">The attributes dictionary to serialize.</param>
    public void SetAttributes(Dictionary<string, object>? attributes)
    {
        if (attributes == null)
        {
            AttributesJson = null;
            return;
        }

        AttributesJson = JsonSerializer.Serialize(attributes);
    }

    /// <summary>
    /// Deserializes the SeoMetadataJson property.
    /// </summary>
    /// <returns>
    /// The deserialized <see cref="SeoMetadata"/> if <see cref="SeoMetadataJson"/> contains valid JSON; otherwise, <see langword="null" />.
    /// </returns>
    public SeoMetadata? GetSeoMetadata()
    {
        if (string.IsNullOrWhiteSpace(SeoMetadataJson))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<SeoMetadata>(SeoMetadataJson);
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
            SeoMetadataJson = null;
            return;
        }

        SeoMetadataJson = JsonSerializer.Serialize(seoMetadata);
    }
}
