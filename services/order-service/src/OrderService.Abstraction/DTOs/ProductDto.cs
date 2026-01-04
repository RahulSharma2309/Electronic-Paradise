namespace OrderService.Abstraction.DTOs;

/// <summary>
/// Represents product information from the Product service.
/// </summary>
public sealed record ProductDto
{
    /// <summary>
    /// Gets the unique identifier of the product.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    public required int Price { get; init; }

    /// <summary>
    /// Gets the available stock quantity.
    /// </summary>
    public required int Stock { get; init; }
}
