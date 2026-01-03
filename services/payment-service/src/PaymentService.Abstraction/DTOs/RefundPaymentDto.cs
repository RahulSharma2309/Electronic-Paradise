namespace PaymentService.Abstraction.DTOs;

public record RefundPaymentDto(
    Guid OrderId, 
    Guid UserId, 
    Guid UserProfileId, 
    decimal Amount
);





