namespace ProductService.Abstraction.Models;

/// <summary>
/// Join entity for many-to-many between products and tags.
/// </summary>
public class ProductTag
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the tag ID.
    /// </summary>
    public Guid TagId { get; set; }

    /// <summary>
    /// Gets or sets the product navigation.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tag navigation.
    /// </summary>
    public Tag Tag { get; set; } = null!;
}
