using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for creating organic certification.
/// Nested within CreateProductRequest or used separately.
/// </summary>
public class CreateCertificationRequest
{
    /// <summary>
    /// Gets or sets the organic certification number.
    /// </summary>
    [Required(ErrorMessage = "Certification number is required")]
    [StringLength(100, ErrorMessage = "Certification number cannot exceed 100 characters")]
    public string CertificationNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of organic certification.
    /// Example: "India Organic", "USDA Organic", "EU Organic".
    /// </summary>
    [Required(ErrorMessage = "Certification type is required")]
    [StringLength(100, ErrorMessage = "Certification type cannot exceed 100 characters")]
    public string CertificationType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the origin or source location.
    /// Example: "Karnataka, India".
    /// </summary>
    [StringLength(200, ErrorMessage = "Origin cannot exceed 200 characters")]
    public string? Origin { get; set; }

    /// <summary>
    /// Gets or sets the certifying agency name.
    /// Example: "APEDA", "USDA".
    /// </summary>
    [StringLength(200, ErrorMessage = "Certifying agency cannot exceed 200 characters")]
    public string? CertifyingAgency { get; set; }

    /// <summary>
    /// Gets or sets the certification issue date.
    /// </summary>
    public DateTime? IssuedDate { get; set; }

    /// <summary>
    /// Gets or sets the certification expiry date.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the product expiration/best-before date (for perishable items).
    /// </summary>
    public DateTime? ProductExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
    public string? Notes { get; set; }
}
