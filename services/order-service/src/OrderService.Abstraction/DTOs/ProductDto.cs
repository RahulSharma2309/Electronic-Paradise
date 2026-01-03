namespace OrderService.Abstraction.DTOs;

public record ProductDto(Guid Id, string Name, string? Description, int Price, int Stock);





