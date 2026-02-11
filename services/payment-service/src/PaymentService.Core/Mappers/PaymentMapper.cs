using PaymentService.Abstraction.DTOs.Requests;
using PaymentService.Abstraction.DTOs.Responses;
using PaymentService.Abstraction.Models;

namespace PaymentService.Core.Mappers;

/// <summary>
/// Implementation of payment mapping between domain models and DTOs.
/// </summary>
public class PaymentMapper : IPaymentMapper
{
    /// <inheritdoc/>
    public PaymentResponse ToResponse(PaymentRecord payment)
    {
        return new PaymentResponse
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            UserId = payment.UserId,
            Amount = payment.Amount,
            Status = payment.Status,
            Timestamp = payment.Timestamp,
        };
    }

    /// <inheritdoc/>
    public PaymentDetailResponse ToDetailResponse(PaymentRecord payment)
    {
        RefundInfo? refundInfo = null;
        if (payment.Status == PaymentStatus.Refunded.ToString())
        {
            refundInfo = new RefundInfo
            {
                Amount = payment.Amount,
                RefundedAt = payment.Timestamp,
                Reason = "Refund processed",
            };
        }

        return new PaymentDetailResponse
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            UserId = payment.UserId,
            Amount = payment.Amount,
            Status = payment.Status,
            Timestamp = payment.Timestamp,
            Metadata = null, // Can be extended in future
            RefundInfo = refundInfo,
        };
    }

    /// <inheritdoc/>
    public PaymentStatusResponse ToStatusResponse(PaymentRecord payment)
    {
        return new PaymentStatusResponse
        {
            PaymentId = payment.Id,
            OrderId = payment.OrderId,
            Status = payment.Status,
            IsSuccessful = payment.Status == PaymentStatus.Success.ToString(),
            LastUpdated = payment.Timestamp,
        };
    }

    /// <inheritdoc/>
    public PaymentRecord ToEntity(ProcessPaymentRequest request, string status = "Pending")
    {
        return new PaymentRecord
        {
            OrderId = request.OrderId,
            UserId = request.UserId,
            Amount = request.Amount,
            Status = status,
            Timestamp = DateTime.UtcNow,
        };
    }
}
