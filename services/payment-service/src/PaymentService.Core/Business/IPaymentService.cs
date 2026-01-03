using PaymentService.Abstraction.DTOs;
using PaymentService.Abstraction.Models;

namespace PaymentService.Core.Business;

public interface IPaymentService
{
    Task<PaymentRecord> ProcessPaymentAsync(ProcessPaymentDto dto);
    Task<PaymentRecord> RefundPaymentAsync(RefundPaymentDto dto);
    Task<PaymentRecord> RecordPaymentAsync(RecordPaymentDto dto);
    Task<PaymentRecord?> GetPaymentStatusAsync(Guid orderId);
}





