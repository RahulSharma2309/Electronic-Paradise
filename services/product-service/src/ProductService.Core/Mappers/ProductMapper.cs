// -----------------------------------------------------------------------
// <copyright file="ProductMapper.cs" company="FreshHarvest-Market">
// Copyright (c) FreshHarvest-Market. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using ProductService.Abstraction.Models;

namespace ProductService.Core.Mappers;

/// <summary>
/// Implementation of product mapping between domain models and DTOs.
/// Simplified for FreshHarvest Market's organic food schema.
/// </summary>
public class ProductMapper : IProductMapper
{
    /// <inheritdoc/>
    public ProductResponse ToResponse(Product product)
    {
        var primaryImageUrl = product.Images
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.SortOrder)
            .Select(i => i.Url)
            .FirstOrDefault();

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Unit = product.Unit,
            Category = product.Category?.Name,
            Brand = product.Brand,
            ImageUrl = primaryImageUrl,

            // Organic fields (inline - no separate table)
            IsOrganic = product.IsOrganic,
            Origin = product.Origin,
            FarmName = product.FarmName,
            BestBefore = product.BestBefore,
            CertificationType = product.CertificationType,

            // Status
            IsActive = product.IsActive,
            IsFeatured = product.IsFeatured,
            CreatedAt = product.CreatedAt,
        };
    }

    /// <inheritdoc/>
    public ProductDetailResponse ToDetailResponse(Product product)
    {
        var primaryImageUrl = product.Images
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.SortOrder)
            .Select(i => i.Url)
            .FirstOrDefault();

        // Map images
        var images = product.Images
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.SortOrder)
            .Select(i => new ProductImageResponse
            {
                Id = i.Id,
                Url = i.Url,
                AltText = i.AltText,
                SortOrder = i.SortOrder,
                IsPrimary = i.IsPrimary,
            })
            .ToList();

        // Map tags from join table
        var tags = product.ProductTags
            .Select(pt => pt.Tag?.Name)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new ProductDetailResponse
        {
            // Core Identity
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Description = product.Description,

            // Pricing & Inventory
            Price = product.Price,
            Stock = product.Stock,
            Unit = product.Unit,
            Sku = product.Sku,

            // Categorization
            CategoryId = product.CategoryId,
            Category = product.Category?.Name,
            Brand = product.Brand,

            // Organic & Freshness (inline fields)
            IsOrganic = product.IsOrganic,
            Origin = product.Origin,
            FarmName = product.FarmName,
            HarvestDate = product.HarvestDate,
            BestBefore = product.BestBefore,

            // Certification (inline fields)
            CertificationNumber = product.CertificationNumber,
            CertificationType = product.CertificationType,
            CertifyingAgency = product.CertifyingAgency,

            // Nutrition
            NutritionJson = product.NutritionJson,

            // SEO (inline fields)
            SeoTitle = product.SeoTitle,
            SeoDescription = product.SeoDescription,

            // Status & Timestamps
            IsActive = product.IsActive,
            IsFeatured = product.IsFeatured,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,

            // Related Data
            ImageUrl = primaryImageUrl,
            Images = images,
            Tags = tags,
        };
    }

    /// <inheritdoc/>
    public Product ToEntity(CreateProductRequest request)
    {
        var product = new Product
        {
            // Core fields
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            Unit = request.Unit,
            Sku = request.Sku,
            Brand = request.Brand,
            CategoryId = request.CategoryId,

            // Organic & Freshness
            IsOrganic = request.IsOrganic,
            Origin = request.Origin,
            FarmName = request.FarmName,
            HarvestDate = request.HarvestDate,
            BestBefore = request.BestBefore,

            // Certification (inline)
            CertificationNumber = request.CertificationNumber,
            CertificationType = request.CertificationType,
            CertifyingAgency = request.CertifyingAgency,

            // Nutrition
            NutritionJson = request.NutritionJson,

            // SEO (inline)
            SeoTitle = request.SeoTitle,
            SeoDescription = request.SeoDescription,

            // Status
            IsActive = request.IsActive,
            IsFeatured = request.IsFeatured,

            // Timestamps
            CreatedAt = DateTime.UtcNow,
        };

        // Map primary image from ImageUrl field
        if (!string.IsNullOrWhiteSpace(request.ImageUrl))
        {
            product.Images.Add(new ProductImage
            {
                ProductId = product.Id,
                Url = request.ImageUrl!,
                IsPrimary = true,
                SortOrder = 0,
                CreatedAt = DateTime.UtcNow,
            });
        }

        return product;
    }

    /// <inheritdoc/>
    public void UpdateEntity(Product product, UpdateProductRequest request)
    {
        // Core fields
        if (request.Name != null)
        {
            product.Name = request.Name;
        }

        if (request.Slug != null)
        {
            product.Slug = request.Slug;
        }

        if (request.Description != null)
        {
            product.Description = request.Description;
        }

        if (request.Price.HasValue)
        {
            product.Price = request.Price.Value;
        }

        if (request.Stock.HasValue)
        {
            product.Stock = request.Stock.Value;
        }

        if (request.Unit != null)
        {
            product.Unit = request.Unit;
        }

        if (request.Sku != null)
        {
            product.Sku = request.Sku;
        }

        if (request.CategoryId.HasValue)
        {
            product.CategoryId = request.CategoryId;
        }

        if (request.Brand != null)
        {
            product.Brand = request.Brand;
        }

        // Organic & Freshness
        if (request.IsOrganic.HasValue)
        {
            product.IsOrganic = request.IsOrganic.Value;
        }

        if (request.Origin != null)
        {
            product.Origin = request.Origin;
        }

        if (request.FarmName != null)
        {
            product.FarmName = request.FarmName;
        }

        if (request.HarvestDate.HasValue)
        {
            product.HarvestDate = request.HarvestDate;
        }

        if (request.BestBefore.HasValue)
        {
            product.BestBefore = request.BestBefore;
        }

        // Certification
        if (request.CertificationNumber != null)
        {
            product.CertificationNumber = request.CertificationNumber;
        }

        if (request.CertificationType != null)
        {
            product.CertificationType = request.CertificationType;
        }

        if (request.CertifyingAgency != null)
        {
            product.CertifyingAgency = request.CertifyingAgency;
        }

        // Nutrition
        if (request.NutritionJson != null)
        {
            product.NutritionJson = request.NutritionJson;
        }

        // SEO
        if (request.SeoTitle != null)
        {
            product.SeoTitle = request.SeoTitle;
        }

        if (request.SeoDescription != null)
        {
            product.SeoDescription = request.SeoDescription;
        }

        // Status
        if (request.IsActive.HasValue)
        {
            product.IsActive = request.IsActive.Value;
        }

        if (request.IsFeatured.HasValue)
        {
            product.IsFeatured = request.IsFeatured.Value;
        }

        // Primary image
        if (request.ImageUrl != null)
        {
            // Mark existing primary images as non-primary
            foreach (var img in product.Images.Where(i => i.IsPrimary))
            {
                img.IsPrimary = false;
            }

            if (!string.IsNullOrWhiteSpace(request.ImageUrl))
            {
                product.Images.Add(new ProductImage
                {
                    ProductId = product.Id,
                    Url = request.ImageUrl,
                    IsPrimary = true,
                    SortOrder = 0,
                    CreatedAt = DateTime.UtcNow,
                });
            }
        }

        product.UpdatedAt = DateTime.UtcNow;
    }
}
