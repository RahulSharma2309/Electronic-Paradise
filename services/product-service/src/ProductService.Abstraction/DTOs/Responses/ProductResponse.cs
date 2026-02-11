// -----------------------------------------------------------------------
// <copyright file="ProductResponse.cs" company="FreshHarvest-Market">
// Copyright (c) FreshHarvest-Market. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ProductService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for product list views (lightweight).
/// Contains essential fields for product listings and cards.
/// Optimized for FreshHarvest Market's organic food marketplace.
/// </summary>
public class ProductResponse
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the SEO-friendly URL slug.
    /// </summary>
    public string Slug { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product description (may be truncated for list views).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price in paise.
    /// Example: 9900 = ₹99.00.
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// Gets or sets the available stock quantity.
    /// </summary>
    public int Stock { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement.
    /// Example: "kg", "dozen", "bunch".
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the brand or farm name.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the primary image URL.
    /// </summary>
    public string? ImageUrl { get; set; }

    // ============================================================================
    // Organic & Freshness (Key display fields for product cards)
    // ============================================================================

    /// <summary>
    /// Gets or sets a value indicating whether this product is certified organic.
    /// Used for organic badge display.
    /// </summary>
    public bool IsOrganic { get; set; }

    /// <summary>
    /// Gets or sets the origin/source location.
    /// Example: "Karnataka, India".
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// Gets or sets the farm or producer name.
    /// </summary>
    public string? FarmName { get; set; }

    /// <summary>
    /// Gets or sets the best before / expiry date.
    /// For freshness indicators.
    /// </summary>
    public DateTime? BestBefore { get; set; }

    /// <summary>
    /// Gets or sets the certification type (for badge display).
    /// Example: "India Organic", "USDA Organic".
    /// </summary>
    public string? CertificationType { get; set; }

    // ============================================================================
    // Status
    // ============================================================================

    /// <summary>
    /// Gets or sets a value indicating whether the product is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is a featured product.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
