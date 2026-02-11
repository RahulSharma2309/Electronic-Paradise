// -----------------------------------------------------------------------
// <copyright file="Product.cs" company="FreshHarvest-Market">
// Copyright (c) FreshHarvest-Market. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ProductService.Abstraction.Models;

/// <summary>
/// Represents a product in the FreshHarvest Market organic food marketplace.
/// Designed for organic produce with freshness tracking and certification support.
/// </summary>
public class Product
{
    // ============================================================================
    // Core Identity
    // ============================================================================

    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the product name.
    /// Example: "Organic Bananas", "Farm Fresh Eggs".
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the SEO-friendly URL slug.
    /// Example: "organic-bananas-kerala", "farm-fresh-eggs-dozen".
    /// </summary>
    public string Slug { get; set; } = null!;

    /// <summary>
    /// Gets or sets the detailed product description.
    /// </summary>
    public string? Description { get; set; }

    // ============================================================================
    // Pricing & Inventory
    // ============================================================================

    /// <summary>
    /// Gets or sets the price in paise (smallest currency unit).
    /// Example: 9900 = ₹99.00.
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// Gets or sets the available stock quantity.
    /// </summary>
    public int Stock { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement.
    /// Example: "kg", "dozen", "bunch", "piece", "500g".
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets the Stock Keeping Unit code.
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
    /// Gets or sets the category navigation property.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// Gets or sets the brand or farm name.
    /// Example: "Happy Farms", "Nature's Best".
    /// </summary>
    public string? Brand { get; set; }

    // ============================================================================
    // Organic & Freshness (Key differentiator for FreshHarvest Market)
    // ============================================================================

    /// <summary>
    /// Gets or sets a value indicating whether this product is certified organic.
    /// </summary>
    public bool IsOrganic { get; set; }

    /// <summary>
    /// Gets or sets the origin/source location.
    /// Example: "Karnataka, India", "Local Farm - Pune".
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// Gets or sets the farm or producer name.
    /// Example: "Green Valley Organics", "Sharma Family Farm".
    /// </summary>
    public string? FarmName { get; set; }

    /// <summary>
    /// Gets or sets the harvest or production date.
    /// Critical for freshness tracking.
    /// </summary>
    public DateTime? HarvestDate { get; set; }

    /// <summary>
    /// Gets or sets the best before / expiry date for perishables.
    /// Null for non-perishable items.
    /// </summary>
    public DateTime? BestBefore { get; set; }

    // ============================================================================
    // Certification (Simplified - key fields only)
    // ============================================================================

    /// <summary>
    /// Gets or sets the organic certification number.
    /// Example: "IN-ORG-123456", "USDA-ORG-789".
    /// Null if not certified.
    /// </summary>
    public string? CertificationNumber { get; set; }

    /// <summary>
    /// Gets or sets the certification type/standard.
    /// Example: "India Organic", "USDA Organic", "EU Organic".
    /// </summary>
    public string? CertificationType { get; set; }

    /// <summary>
    /// Gets or sets the certifying agency.
    /// Example: "APEDA", "Ecocert", "Control Union".
    /// </summary>
    public string? CertifyingAgency { get; set; }

    // ============================================================================
    // Nutrition (JSON for flexibility)
    // ============================================================================

    /// <summary>
    /// Gets or sets nutrition information as JSON.
    /// Example: {"calories": 89, "protein": 1.1, "carbs": 23, "fiber": 2.6}.
    /// </summary>
    public string? NutritionJson { get; set; }

    // ============================================================================
    // SEO (Inline - no separate table needed)
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
    /// Gets or sets a value indicating whether the product is active/visible.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this is a featured product.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last update timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // ============================================================================
    // Navigation Properties (Related Entities)
    // ============================================================================

    /// <summary>
    /// Gets or sets the product images (gallery).
    /// </summary>
    public List<ProductImage> Images { get; set; } = new();

    /// <summary>
    /// Gets or sets the product tags (many-to-many via ProductTags).
    /// </summary>
    public List<ProductTag> ProductTags { get; set; } = new();
}
