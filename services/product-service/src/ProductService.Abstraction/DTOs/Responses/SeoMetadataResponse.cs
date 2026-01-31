namespace ProductService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for SEO metadata.
/// </summary>
public class SeoMetadataResponse
{
    /// <summary>
    /// Gets or sets the SEO title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the SEO description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets SEO keywords.
    /// </summary>
    public string[]? Keywords { get; set; }

    /// <summary>
    /// Gets or sets the canonical URL.
    /// </summary>
    public string? CanonicalUrl { get; set; }
}
