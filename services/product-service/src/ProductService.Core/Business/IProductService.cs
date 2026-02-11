using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;

namespace ProductService.Core.Business;

/// <summary>
/// Defines the contract for product-related business operations.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieves all products (lightweight for lists).
    /// </summary>
    /// <returns>A list of all products.</returns>
    Task<List<ProductResponse>> ListAsync();

    /// <summary>
    /// Retrieves a product by its unique identifier (comprehensive with all details).
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product if found, otherwise null.</returns>
    Task<ProductDetailResponse?> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new product after validation.
    /// </summary>
    /// <param name="request">The product creation request.</param>
    /// <returns>The created product details.</returns>
    /// <exception cref="ArgumentException">Thrown if validation fails.</exception>
    Task<ProductDetailResponse> CreateAsync(CreateProductRequest request);

    /// <summary>
    /// Reserves a specified quantity of stock for a product (decreases stock).
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="quantity">The quantity to reserve.</param>
    /// <returns>The new stock quantity after reservation.</returns>
    /// <exception cref="ArgumentException">Thrown if quantity is not positive.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the product is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if there is insufficient stock.</exception>
    Task<int> ReserveAsync(Guid id, int quantity);

    /// <summary>
    /// Releases a specified quantity of previously reserved stock (increases stock).
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="quantity">The quantity to release.</param>
    /// <returns>The new stock quantity after release.</returns>
    /// <exception cref="ArgumentException">Thrown if quantity is not positive.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the product is not found.</exception>
    Task<int> ReleaseAsync(Guid id, int quantity);
}
