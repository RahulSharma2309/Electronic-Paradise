// -----------------------------------------------------------------------
// <copyright file="ProductImageResponse.cs" company="FreshHarvest-Market">
// Copyright (c) FreshHarvest-Market. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ProductService.Abstraction.DTOs.Responses;

/// <summary>
/// Response DTO for product images.
/// </summary>
public class ProductImageResponse
{
    /// <summary>
    /// Gets or sets the image ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// Gets or sets the alt text.
    /// </summary>
    public string? AltText { get; set; }

    /// <summary>
    /// Gets or sets the sort order.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is the primary image.
    /// </summary>
    public bool IsPrimary { get; set; }
}
