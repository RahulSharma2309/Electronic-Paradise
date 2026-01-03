namespace OrderService.Abstraction.DTOs;

public record UserProfileDto(Guid Id, Guid UserId, string? FirstName, string? LastName, string? PhoneNumber, decimal WalletBalance);





