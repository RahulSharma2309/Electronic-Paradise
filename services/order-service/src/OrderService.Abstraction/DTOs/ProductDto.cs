namespace OrderService.Abstraction.DTOs;

/// <summary>
/// Represents product information from the Product service.
/// Enhanced with organic and extensibility fields.
/// </summary>
public sealed record ProductDto
{
    // Core Fields

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
    /// Gets the price of the product (in smallest currency unit).
    /// </summary>
    public required int Price { get; init; }

    /// <summary>
    /// Gets the available stock quantity.
    /// </summary>
    public required int Stock { get; init; }

    // Organic/Category Fields

    /// <summary>
    /// Gets the product category.
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// Gets the brand or manufacturer name.
    /// </summary>
    public string? Brand { get; init; }

    /// <summary>
    /// Gets the origin or source location.
    /// </summary>
    public string? Origin { get; init; }

    /// <summary>
    /// Gets the organic certification number.
    /// </summary>
    public string? CertificationNumber { get; init; }

    /// <summary>
    /// Gets the type of organic certification.
    /// </summary>
    public string? CertificationType { get; init; }

    /// <summary>
    /// Gets the unit of measurement.
    /// </summary>
    public string? Unit { get; init; }

    /// <summary>
    /// Gets the expiration date.
    /// </summary>
    public DateTime? ExpirationDate { get; init; }

    // Extensibility Fields

    /// <summary>
    /// Gets the SKU.
    /// </summary>
    public string? Sku { get; init; }

    /// <summary>
    /// Gets the image URL.
    /// </summary>
    public string? ImageUrl { get; init; }
}
