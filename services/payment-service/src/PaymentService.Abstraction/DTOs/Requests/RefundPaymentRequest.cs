using System.ComponentModel.DataAnnotations;

namespace PaymentService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for refunding a payment.
/// Contains validation attributes for API layer.
/// </summary>
public class RefundPaymentRequest
{
    /// <summary>
    /// Gets or sets the payment ID to refund.
    /// </summary>
    [Required(ErrorMessage = "Payment ID is required")]
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Gets or sets the reason for refund (optional).
    /// </summary>
    [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}
