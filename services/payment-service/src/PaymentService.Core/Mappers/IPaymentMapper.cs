using PaymentService.Abstraction.DTOs.Requests;
using PaymentService.Abstraction.DTOs.Responses;
using PaymentService.Abstraction.Models;

namespace PaymentService.Core.Mappers;

/// <summary>
/// Defines the contract for mapping between PaymentRecord domain models and DTOs.
/// </summary>
public interface IPaymentMapper
{
    /// <summary>
    /// Maps a PaymentRecord domain model to a lightweight PaymentResponse DTO.
    /// </summary>
    /// <param name="payment">The payment record domain model.</param>
    /// <returns>The payment response DTO.</returns>
    PaymentResponse ToResponse(PaymentRecord payment);

    /// <summary>
    /// Maps a PaymentRecord domain model to a comprehensive PaymentDetailResponse DTO.
    /// </summary>
    /// <param name="payment">The payment record domain model.</param>
    /// <returns>The payment detail response DTO.</returns>
    PaymentDetailResponse ToDetailResponse(PaymentRecord payment);

    /// <summary>
    /// Maps a PaymentRecord domain model to a PaymentStatusResponse DTO.
    /// </summary>
    /// <param name="payment">The payment record domain model.</param>
    /// <returns>The payment status response DTO.</returns>
    PaymentStatusResponse ToStatusResponse(PaymentRecord payment);

    /// <summary>
    /// Maps a ProcessPaymentRequest DTO to a PaymentRecord domain model.
    /// </summary>
    /// <param name="request">The process payment request DTO.</param>
    /// <param name="status">The initial payment status.</param>
    /// <returns>The payment record domain model.</returns>
    PaymentRecord ToEntity(ProcessPaymentRequest request, string status = "Pending");
}
