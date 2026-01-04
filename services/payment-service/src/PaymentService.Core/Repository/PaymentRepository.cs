using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentService.Abstraction.Models;
using PaymentService.Core.Data;

namespace PaymentService.Core.Repository;

/// <summary>
/// Provides data access operations for <see cref="PaymentRecord"/> entities.
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<PaymentRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentRepository"/> class.
    /// </summary>
    /// <param name="db">The application database context.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="db"/> or <paramref name="logger"/> is null.</exception>
    public PaymentRepository(AppDbContext db, ILogger<PaymentRepository> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<PaymentRecord> AddAsync(PaymentRecord payment)
    {
        _logger.LogDebug("Adding payment record for Order {OrderId}, Amount: {Amount}", payment.OrderId, payment.Amount);

        try
        {
            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Payment record added successfully. Payment ID: {PaymentId}, Order ID: {OrderId}", payment.Id, payment.OrderId);
            return payment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add payment record for Order {OrderId}", payment.OrderId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentRecord?> GetByOrderIdAsync(Guid orderId)
    {
        _logger.LogDebug("Fetching payment record for Order {OrderId}", orderId);

        try
        {
            var payment = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            if (payment == null)
            {
                _logger.LogDebug("Payment record not found for Order {OrderId}", orderId);
            }
            else
            {
                _logger.LogDebug("Payment record found for Order {OrderId}: Payment ID {PaymentId}, Status {Status}", orderId, payment.Id, payment.Status);
            }

            return payment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching payment record for Order {OrderId}", orderId);
            throw;
        }
    }
}
