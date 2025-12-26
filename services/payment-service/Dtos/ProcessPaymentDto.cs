namespace PaymentService.Dtos;

public record ProcessPaymentDto(
    Guid OrderId, 
    Guid UserId, 
    Guid UserProfileId, 
    decimal Amount
);

