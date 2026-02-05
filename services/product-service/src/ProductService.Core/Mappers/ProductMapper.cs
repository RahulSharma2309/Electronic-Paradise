using System.Text.Json;
using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using ProductService.Abstraction.Models;

namespace ProductService.Core.Mappers;

/// <summary>
/// Implementation of product mapping between domain models and DTOs.
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
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category?.Name,
            Brand = product.Brand,
            Unit = product.Unit,
            ImageUrl = primaryImageUrl,
            IsActive = product.IsActive,
            HasCertification = product.Certification != null,
            CertificationType = product.Certification?.CertificationType,
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

        CertificationResponse? certification = null;
        if (product.Certification != null)
        {
            certification = new CertificationResponse
            {
                Id = product.Certification.Id,
                CertificationNumber = product.Certification.CertificationNumber,
                CertificationType = product.Certification.CertificationType,
                Origin = product.Certification.Origin,
                CertifyingAgency = product.Certification.CertifyingAgency,
                IssuedDate = product.Certification.IssuedDate,
                ExpiryDate = product.Certification.ExpiryDate,
                IsValid = product.Certification.IsValid,
                ProductExpirationDate = product.Certification.ProductExpirationDate,
                Notes = product.Certification.Notes,
            };
        }

        MetadataResponse? metadata = null;
        if (product.Metadata != null)
        {
            SeoMetadataResponse? seoMetadata = null;
            var seo = product.Metadata.GetSeoMetadata();
            if (seo != null)
            {
                seoMetadata = new SeoMetadataResponse
                {
                    Title = seo.Title,
                    Description = seo.Description,
                    Keywords = seo.Keywords,
                    CanonicalUrl = seo.CanonicalUrl,
                };
            }

            metadata = new MetadataResponse
            {
                Id = product.Metadata.Id,
                Slug = product.Metadata.Slug,
                SeoMetadata = seoMetadata,
            };
        }

        // Build tags + attributes from normalized tables (keeps response contract stable).
        var tags = product.ProductTags
            .Select(pt => pt.Tag?.Name)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var attributes = product.Attributes.Count == 0
            ? null
            : product.Attributes
                .Select(a =>
                {
                    object? value = a.ValueString != null
                        ? a.ValueString
                        : a.ValueNumber.HasValue
                            ? a.ValueNumber.Value
                            : a.ValueBoolean.HasValue
                                ? a.ValueBoolean.Value
                                : null;

                    return new { a.Key, Value = value };
                })
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Key, x => x.Value!, StringComparer.OrdinalIgnoreCase);

        if (attributes != null && attributes.Count == 0)
        {
            attributes = null;
        }

        if (metadata == null && (tags.Length > 0 || attributes != null))
        {
            metadata = new MetadataResponse
            {
                Id = Guid.Empty,
            };
        }

        if (metadata != null)
        {
            metadata.Tags = tags.Length == 0 ? null : tags;
            metadata.Attributes = attributes;
        }

        return new ProductDetailResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category?.Name,
            Brand = product.Brand,
            Sku = product.Sku,
            Unit = product.Unit,
            ImageUrl = primaryImageUrl,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            Certification = certification,
            Metadata = metadata,
        };
    }

    /// <inheritdoc/>
    public Product ToEntity(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            Brand = request.Brand,
            Sku = request.Sku,
            Unit = request.Unit,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };

        // Map certification if provided
        if (request.Certification != null)
        {
            product.Certification = new ProductCertification
            {
                ProductId = product.Id,
                CertificationNumber = request.Certification.CertificationNumber,
                CertificationType = request.Certification.CertificationType,
                Origin = request.Certification.Origin,
                CertifyingAgency = request.Certification.CertifyingAgency,
                IssuedDate = request.Certification.IssuedDate,
                ExpiryDate = request.Certification.ExpiryDate,
                IsValid = true, // Default to valid when creating
                ProductExpirationDate = request.Certification.ProductExpirationDate,
                Notes = request.Certification.Notes,
                CreatedAt = DateTime.UtcNow,
            };
        }

        // Map metadata if provided (SEO/slug)
        if (request.Metadata != null)
        {
            product.Metadata = new ProductMetadata
            {
                ProductId = product.Id,
                Slug = request.Metadata.Slug,
                CreatedAt = DateTime.UtcNow,
            };

            // Set SEO metadata if provided
            if (request.Metadata.SeoMetadata != null)
            {
                product.Metadata.SetSeoMetadata(new SeoMetadata
                {
                    Title = request.Metadata.SeoMetadata.Title,
                    Description = request.Metadata.SeoMetadata.Description,
                    Keywords = request.Metadata.SeoMetadata.Keywords,
                    CanonicalUrl = request.Metadata.SeoMetadata.CanonicalUrl,
                });
            }
        }

        // Map primary image from legacy ImageUrl field (stored as ProductImage row)
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

        // Map flexible attributes (legacy request.Metadata.Attributes -> ProductAttributes)
        if (request.Metadata?.Attributes != null && request.Metadata.Attributes.Count > 0)
        {
            foreach (var kvp in request.Metadata.Attributes)
            {
                var value = kvp.Value;
                var attr = new ProductAttribute
                {
                    ProductId = product.Id,
                    Key = kvp.Key,
                    Group = "General",
                    CreatedAt = DateTime.UtcNow,
                };

                switch (value)
                {
                    case null:
                        break;
                    case bool b:
                        attr.ValueBoolean = b;
                        break;
                    case byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal:
                        attr.ValueNumber = Convert.ToDecimal(value);
                        break;
                    case string s:
                        attr.ValueString = s;
                        break;
                    default:
                        // For arrays/objects, store JSON for now (still queryable by key).
                        attr.ValueString = JsonSerializer.Serialize(value);
                        break;
                }

                product.Attributes.Add(attr);
            }
        }

        return product;
    }

    /// <inheritdoc/>
    public void UpdateEntity(Product product, UpdateProductRequest request)
    {
        // Update only provided fields (PATCH semantics)
        if (request.Name != null)
        {
            product.Name = request.Name;
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

        if (request.Brand != null)
        {
            product.Brand = request.Brand;
        }

        if (request.Sku != null)
        {
            product.Sku = request.Sku;
        }

        if (request.Unit != null)
        {
            product.Unit = request.Unit;
        }

        if (request.ImageUrl != null)
        {
            // Update primary image (store in ProductImages table)
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

        if (request.IsActive.HasValue)
        {
            product.IsActive = request.IsActive.Value;
        }

        product.UpdatedAt = DateTime.UtcNow;
    }
}
