namespace PaymentService.Dtos;

public record RefundPaymentDto(
    Guid OrderId, 
    Guid UserId, 
    Guid UserProfileId, 
    decimal Amount
);

