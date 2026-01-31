namespace ProductService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for product metadata.
/// Used within ProductDetailResponse.
/// </summary>
public class MetadataResponse
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets custom attributes as dictionary.
    /// Deserialized from JSON for easy consumption by frontend.
    /// </summary>
    public Dictionary<string, object>? Attributes { get; set; }

    /// <summary>
    /// Gets or sets custom tags.
    /// </summary>
    public string[]? Tags { get; set; }

    /// <summary>
    /// Gets or sets the SEO-friendly slug.
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the SEO metadata.
    /// </summary>
    public SeoMetadataResponse? SeoMetadata { get; set; }
}
