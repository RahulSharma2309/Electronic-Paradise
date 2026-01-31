namespace ProductService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for product certification details.
/// Used within ProductDetailResponse.
/// </summary>
public class CertificationResponse
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the certification number.
    /// </summary>
    public string CertificationNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the certification type.
    /// Example: "India Organic", "USDA Organic", "EU Organic".
    /// </summary>
    public string CertificationType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the origin/source location.
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// Gets or sets the certifying agency.
    /// </summary>
    public string? CertifyingAgency { get; set; }

    /// <summary>
    /// Gets or sets the issue date.
    /// </summary>
    public DateTime? IssuedDate { get; set; }

    /// <summary>
    /// Gets or sets the expiry date.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the certification is currently valid.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the product expiration date (for perishable items).
    /// </summary>
    public DateTime? ProductExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    public string? Notes { get; set; }
}
