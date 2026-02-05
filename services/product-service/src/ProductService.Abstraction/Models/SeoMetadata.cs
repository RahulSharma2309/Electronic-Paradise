namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents SEO metadata for a product.
/// Separate class for type safety and better code organization.
/// </summary>
public class SeoMetadata
{
    /// <summary>
    /// Gets or sets the SEO title (for page title tag).
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the SEO description (for meta description tag).
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
