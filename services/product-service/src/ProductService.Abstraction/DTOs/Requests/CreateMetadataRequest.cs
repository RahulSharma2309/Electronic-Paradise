using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for creating product metadata.
/// Nested within CreateProductRequest or used separately.
/// </summary>
public class CreateMetadataRequest
{
    /// <summary>
    /// Gets or sets custom attributes as dictionary.
    /// Will be serialized to JSON for storage.
    /// Example: {"organic": true, "gmo_free": true, "allergens": ["nuts"]}.
    /// </summary>
    public Dictionary<string, object>? Attributes { get; set; }

    /// <summary>
    /// Gets or sets custom tags for search and categorization.
    /// Example: ["organic", "local", "seasonal"].
    /// </summary>
    public string[]? Tags { get; set; }

    /// <summary>
    /// Gets or sets SEO-friendly slug.
    /// Example: "organic-bananas-kerala".
    /// </summary>
    [StringLength(200, ErrorMessage = "Slug cannot exceed 200 characters")]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug must contain only lowercase letters, numbers, and hyphens")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets SEO metadata.
    /// </summary>
    public SeoMetadataRequest? SeoMetadata { get; set; }
}
