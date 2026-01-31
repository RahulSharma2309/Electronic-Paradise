using System.ComponentModel.DataAnnotations;

namespace PaymentService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for processing a payment.
/// Contains validation attributes for API layer.
/// </summary>
public class ProcessPaymentRequest
{
    /// <summary>
    /// Gets or sets the order ID for this payment.
    /// </summary>
    [Required(ErrorMessage = "Order ID is required")]
    public Guid OrderId { get; set; }

    /// <summary>
    /// Gets or sets the user ID making the payment.
    /// </summary>
    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the payment amount.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }
}
