using PaymentService.Abstraction.Models;

namespace PaymentService.Core.Repository;

public interface IPaymentRepository
{
    Task<PaymentRecord> AddAsync(PaymentRecord payment);
    Task<PaymentRecord?> GetByOrderIdAsync(Guid orderId);
}





