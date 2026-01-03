using Microsoft.EntityFrameworkCore;
using PaymentService.Abstraction.Models;
using PaymentService.Core.Data;

namespace PaymentService.Core.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _db;
    public PaymentRepository(AppDbContext db) => _db = db;

    public async Task<PaymentRecord> AddAsync(PaymentRecord payment)
    {
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();
        return payment;
    }

    public async Task<PaymentRecord?> GetByOrderIdAsync(Guid orderId)
    {
        return await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
    }
}





