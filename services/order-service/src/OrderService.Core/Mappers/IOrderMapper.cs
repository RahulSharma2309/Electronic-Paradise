using OrderService.Abstraction.DTOs.Requests;
using OrderService.Abstraction.DTOs.Responses;
using OrderService.Abstraction.Models;

namespace OrderService.Core.Mappers;

/// <summary>
/// Defines the contract for mapping between Order domain models and DTOs.
/// </summary>
public interface IOrderMapper
{
    /// <summary>
    /// Maps an Order domain model to a lightweight OrderResponse DTO.
    /// </summary>
    /// <param name="order">The order domain model.</param>
    /// <returns>The order response DTO.</returns>
    OrderResponse ToResponse(Order order);

    /// <summary>
    /// Maps an Order domain model with all items to a comprehensive OrderDetailResponse DTO.
    /// </summary>
    /// <param name="order">The order domain model with items loaded.</param>
    /// <returns>The order detail response DTO.</returns>
    OrderDetailResponse ToDetailResponse(Order order);

    /// <summary>
    /// Maps a CreateOrderRequest DTO to an Order domain model.
    /// </summary>
    /// <param name="request">The create order request DTO.</param>
    /// <returns>The order domain model.</returns>
    Order ToEntity(CreateOrderRequest request);
}
