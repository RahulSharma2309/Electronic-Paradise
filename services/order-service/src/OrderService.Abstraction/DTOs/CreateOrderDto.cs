namespace OrderService.Abstraction.DTOs;

public record CreateOrderDto(Guid UserId, List<OrderItemDto> Items);




