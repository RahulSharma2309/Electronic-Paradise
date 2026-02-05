namespace PaymentService.Abstraction.Models;

/// <summary>
/// Represents the status of a payment transaction.
/// Type-safe enum instead of string for better validation and consistency.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment is pending processing.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Payment completed successfully.
    /// </summary>
    Success = 1,

    /// <summary>
    /// Payment failed.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// Payment was refunded.
    /// </summary>
    Refunded = 3,

    /// <summary>
    /// Payment is being processed.
    /// </summary>
    Processing = 4,

    /// <summary>
    /// Payment was cancelled before completion.
    /// </summary>
    Cancelled = 5,
}
