namespace PaymentService.Abstraction.DTOs;

public record ProcessPaymentDto(
    Guid OrderId, 
    Guid UserId, 
    Guid UserProfileId, 
    decimal Amount
);





