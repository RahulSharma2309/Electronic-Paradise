namespace ProductService.Abstraction.DTOs;

/// <summary>
/// Represents the data transfer object for releasing reserved stock.
/// </summary>
public sealed record ReleaseDto
{
    /// <summary>
    /// Gets the quantity of stock to release.
    /// </summary>
    required public int Quantity { get; init; }
}
