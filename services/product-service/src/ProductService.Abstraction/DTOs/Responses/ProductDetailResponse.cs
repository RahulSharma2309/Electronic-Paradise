// -----------------------------------------------------------------------
// <copyright file="ProductDetailResponse.cs" company="FreshHarvest-Market">
// Copyright (c) FreshHarvest-Market. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ProductService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for product detail views (comprehensive).
/// Contains all product information for product detail pages.
/// Simplified schema - no nested objects, all fields inline.
/// </summary>
public class ProductDetailResponse
{
    // ============================================================================
    // Core Identity
    // ============================================================================

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
    /// Gets or sets the product description.
    /// </summary>
    public string? Description { get; set; }

    // ============================================================================
    // Pricing & Inventory
    // ============================================================================

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
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets the SKU.
    /// </summary>
    public string? Sku { get; set; }

    // ============================================================================
    // Categorization
    // ============================================================================

    /// <summary>
    /// Gets or sets the category ID.
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the brand or farm name.
    /// </summary>
    public string? Brand { get; set; }

    // ============================================================================
    // Organic & Freshness
    // ============================================================================

    /// <summary>
    /// Gets or sets a value indicating whether this product is certified organic.
    /// </summary>
    public bool IsOrganic { get; set; }

    /// <summary>
    /// Gets or sets the origin/source location.
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// Gets or sets the farm or producer name.
    /// </summary>
    public string? FarmName { get; set; }

    /// <summary>
    /// Gets or sets the harvest or production date.
    /// </summary>
    public DateTime? HarvestDate { get; set; }

    /// <summary>
    /// Gets or sets the best before / expiry date.
    /// </summary>
    public DateTime? BestBefore { get; set; }

    // ============================================================================
    // Certification
    // ============================================================================

    /// <summary>
    /// Gets or sets the organic certification number.
    /// </summary>
    public string? CertificationNumber { get; set; }

    /// <summary>
    /// Gets or sets the certification type/standard.
    /// </summary>
    public string? CertificationType { get; set; }

    /// <summary>
    /// Gets or sets the certifying agency.
    /// </summary>
    public string? CertifyingAgency { get; set; }

    // ============================================================================
    // Nutrition
    // ============================================================================

    /// <summary>
    /// Gets or sets nutrition information as JSON string.
    /// Frontend should parse this for display.
    /// </summary>
    public string? NutritionJson { get; set; }

    // ============================================================================
    // SEO
    // ============================================================================

    /// <summary>
    /// Gets or sets the SEO meta title.
    /// </summary>
    public string? SeoTitle { get; set; }

    /// <summary>
    /// Gets or sets the SEO meta description.
    /// </summary>
    public string? SeoDescription { get; set; }

    // ============================================================================
    // Status & Timestamps
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

    /// <summary>
    /// Gets or sets the last update timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // ============================================================================
    // Related Data
    // ============================================================================

    /// <summary>
    /// Gets or sets the primary image URL.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets all product images.
    /// </summary>
    public List<ProductImageResponse> Images { get; set; } = new();

    /// <summary>
    /// Gets or sets the product tags.
    /// </summary>
    public List<string> Tags { get; set; } = new();
}
