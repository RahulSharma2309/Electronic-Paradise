// -----------------------------------------------------------------------
// <copyright file="UpdateProductRequest.cs" company="FreshHarvest-Market">
// Copyright (c) FreshHarvest-Market. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs.Requests;

/// <summary>
/// Request DTO for updating an existing product.
/// All fields are optional - only provided fields will be updated (PATCH semantics).
/// </summary>
public class UpdateProductRequest
{
    // ============================================================================
    // Core Fields
    // ============================================================================

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    [StringLength(300, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 300 characters")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the SEO-friendly URL slug.
    /// </summary>
    [StringLength(300, ErrorMessage = "Slug cannot exceed 300 characters")]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug must contain only lowercase letters, numbers, and hyphens")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the product in paise.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Price must be non-negative")]
    public int? Price { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
    public int? Stock { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement.
    /// </summary>
    [StringLength(50, ErrorMessage = "Unit cannot exceed 50 characters")]
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets the SKU.
    /// </summary>
    [StringLength(100, ErrorMessage = "SKU cannot exceed 100 characters")]
    public string? Sku { get; set; }

    /// <summary>
    /// Gets or sets the category ID.
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the brand or farm name.
    /// </summary>
    [StringLength(150, ErrorMessage = "Brand cannot exceed 150 characters")]
    public string? Brand { get; set; }

    // ============================================================================
    // Organic & Freshness Fields
    // ============================================================================

    /// <summary>
    /// Gets or sets a value indicating whether this product is certified organic.
    /// </summary>
    public bool? IsOrganic { get; set; }

    /// <summary>
    /// Gets or sets the origin/source location.
    /// </summary>
    [StringLength(200, ErrorMessage = "Origin cannot exceed 200 characters")]
    public string? Origin { get; set; }

    /// <summary>
    /// Gets or sets the farm or producer name.
    /// </summary>
    [StringLength(200, ErrorMessage = "Farm name cannot exceed 200 characters")]
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
    // Certification Fields
    // ============================================================================

    /// <summary>
    /// Gets or sets the organic certification number.
    /// </summary>
    [StringLength(100, ErrorMessage = "Certification number cannot exceed 100 characters")]
    public string? CertificationNumber { get; set; }

    /// <summary>
    /// Gets or sets the certification type/standard.
    /// </summary>
    [StringLength(100, ErrorMessage = "Certification type cannot exceed 100 characters")]
    public string? CertificationType { get; set; }

    /// <summary>
    /// Gets or sets the certifying agency.
    /// </summary>
    [StringLength(200, ErrorMessage = "Certifying agency cannot exceed 200 characters")]
    public string? CertifyingAgency { get; set; }

    // ============================================================================
    // Nutrition
    // ============================================================================

    /// <summary>
    /// Gets or sets nutrition information as JSON string.
    /// </summary>
    [StringLength(4000, ErrorMessage = "Nutrition JSON cannot exceed 4000 characters")]
    public string? NutritionJson { get; set; }

    // ============================================================================
    // SEO Fields
    // ============================================================================

    /// <summary>
    /// Gets or sets the SEO meta title.
    /// </summary>
    [StringLength(200, ErrorMessage = "SEO title cannot exceed 200 characters")]
    public string? SeoTitle { get; set; }

    /// <summary>
    /// Gets or sets the SEO meta description.
    /// </summary>
    [StringLength(500, ErrorMessage = "SEO description cannot exceed 500 characters")]
    public string? SeoDescription { get; set; }

    // ============================================================================
    // Status Fields
    // ============================================================================

    /// <summary>
    /// Gets or sets a value indicating whether the product is active/visible.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is a featured product.
    /// </summary>
    public bool? IsFeatured { get; set; }

    // ============================================================================
    // Related Data
    // ============================================================================

    /// <summary>
    /// Gets or sets the primary image URL.
    /// </summary>
    [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
    [Url(ErrorMessage = "Image URL must be a valid URL")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets tag slugs to associate with this product.
    /// Replaces existing tags.
    /// </summary>
    public string[]? Tags { get; set; }
}
