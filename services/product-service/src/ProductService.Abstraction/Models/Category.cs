namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents a product category (taxonomy) for organic catalog browsing.
/// </summary>
public class Category
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the category name (e.g., "Fruits", "Vegetables").
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the SEO-friendly unique slug (e.g., "fruits", "leafy-greens").
    /// </summary>
    public string Slug { get; set; } = null!;

    /// <summary>
    /// Gets or sets an optional description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the optional parent category for hierarchy (e.g., "Vegetables" -> "Leafy Greens").
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the parent category.
    /// </summary>
    public Category? Parent { get; set; }

    /// <summary>
    /// Gets or sets the child categories.
    /// </summary>
    public List<Category> Children { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the category is active/visible.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last updated timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets products in this category.
    /// </summary>
    public List<Product> Products { get; set; } = new();
}
