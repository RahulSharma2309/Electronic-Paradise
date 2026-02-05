using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using ProductService.Abstraction.Models;

namespace ProductService.Core.Mappers;

/// <summary>
/// Defines the contract for mapping between Product domain models and DTOs.
/// </summary>
public interface IProductMapper
{
    /// <summary>
    /// Maps a Product domain model to a lightweight ProductResponse DTO.
    /// </summary>
    /// <param name="product">The product domain model.</param>
    /// <returns>The product response DTO.</returns>
    ProductResponse ToResponse(Product product);

    /// <summary>
    /// Maps a Product domain model with all related entities to a comprehensive ProductDetailResponse DTO.
    /// </summary>
    /// <param name="product">The product domain model with related entities loaded.</param>
    /// <returns>The product detail response DTO.</returns>
    ProductDetailResponse ToDetailResponse(Product product);

    /// <summary>
    /// Maps a CreateProductRequest DTO to a Product domain model.
    /// </summary>
    /// <param name="request">The create product request DTO.</param>
    /// <returns>The product domain model.</returns>
    Product ToEntity(CreateProductRequest request);

    /// <summary>
    /// Updates an existing Product domain model with data from an UpdateProductRequest DTO.
    /// Only updates fields that are provided (not null).
    /// </summary>
    /// <param name="product">The existing product domain model to update.</param>
    /// <param name="request">The update product request DTO.</param>
    void UpdateEntity(Product product, UpdateProductRequest request);
}
