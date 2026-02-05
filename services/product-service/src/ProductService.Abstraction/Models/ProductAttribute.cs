namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents a queryable product attribute/specification (EAV-style).
/// For organic products, this can store things like "harvestDate", "origin", "isPesticideFree", etc.
/// </summary>
public class ProductAttribute
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the product ID this attribute belongs to.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the attribute key (e.g., "harvestDate", "soilType").
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// Gets or sets the optional group for UI (e.g., "Origin", "Farming").
    /// </summary>
    public string? Group { get; set; }

    /// <summary>
    /// Gets or sets the optional unit (e.g., "days", "km").
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets string value (if applicable).
    /// </summary>
    public string? ValueString { get; set; }

    /// <summary>
    /// Gets or sets numeric value (if applicable).
    /// </summary>
    public decimal? ValueNumber { get; set; }

    /// <summary>
    /// Gets or sets boolean value (if applicable).
    /// </summary>
    public bool? ValueBoolean { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the product navigation.
    /// </summary>
    public Product Product { get; set; } = null!;
}
