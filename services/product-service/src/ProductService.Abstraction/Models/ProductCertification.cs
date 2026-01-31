namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents organic certification details for a product.
/// Separated from Product entity following Single Responsibility Principle.
/// Specific to organic products domain.
/// </summary>
public class ProductCertification
{
    /// <summary>
    /// Gets or sets the unique identifier for the certification record.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the product ID this certification belongs to.
    /// Foreign key to Product entity.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the organic certification number.
    /// Example: "IN-ORG-123456", "USDA-ORG-789012".
    /// </summary>
    public string CertificationNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of organic certification.
    /// Example: "India Organic", "USDA Organic", "EU Organic".
    /// </summary>
    public string CertificationType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the origin or source location of the product.
    /// Example: "Karnataka, India", "California, USA", "Local Farm".
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// Gets or sets the certifying agency name.
    /// Example: "APEDA", "USDA", "Ecocert".
    /// </summary>
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
    /// Gets or sets a value indicating whether the certification is currently valid.
    /// Can be computed or manually set.
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// Gets or sets the expiration or best-before date for the product itself (for perishable items).
    /// Null for non-perishable products.
    /// </summary>
    public DateTime? ProductExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets additional notes or verification details.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this certification record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last updated timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // ============================================================================
    // Navigation Properties
    // ============================================================================

    /// <summary>
    /// Gets or sets the product this certification belongs to.
    /// Navigation property for EF Core.
    /// </summary>
    public Product Product { get; set; } = null!;
}
