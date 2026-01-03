namespace OrderService.Abstraction.DTOs;

/// <summary>
/// Represents product information from the Product service.
/// </summary>
public sealed record ProductDto
{
    /// <summary>
    /// Gets the unique identifier of the product.
    /// </summary>
    required public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    required public string Name { get; init; }

    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    required public int Price { get; init; }

    /// <summary>
    /// Gets the available stock quantity.
    /// </summary>
    required public int Stock { get; init; }
}
