namespace PaymentService.Abstraction.DTOs;

public record RecordPaymentDto(Guid OrderId, Guid UserId, decimal Amount, string Status);





