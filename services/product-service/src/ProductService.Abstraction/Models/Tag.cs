namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents a reusable tag for product discovery (e.g., "organic", "local", "seasonal").
/// </summary>
public class Tag
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets a unique slug for case-insensitive matching.
    /// </summary>
    public string Slug { get; set; } = null!;

    /// <summary>
    /// Gets or sets product tag join rows.
    /// </summary>
    public List<ProductTag> ProductTags { get; set; } = new();
}
