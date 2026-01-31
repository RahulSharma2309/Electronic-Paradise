using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for SEO metadata.
/// </summary>
public class SeoMetadataRequest
{
    /// <summary>
    /// Gets or sets the SEO title.
    /// </summary>
    [StringLength(100, ErrorMessage = "SEO title cannot exceed 100 characters")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the SEO description.
    /// </summary>
    [StringLength(300, ErrorMessage = "SEO description cannot exceed 300 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the SEO keywords.
    /// </summary>
    public string[]? Keywords { get; set; }

    /// <summary>
    /// Gets or sets the canonical URL.
    /// </summary>
    [Url(ErrorMessage = "Canonical URL must be a valid URL")]
    public string? CanonicalUrl { get; set; }
}
