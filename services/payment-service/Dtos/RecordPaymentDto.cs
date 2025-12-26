namespace PaymentService.Dtos;

public record RecordPaymentDto(Guid OrderId, Guid UserId, decimal Amount, string Status);
