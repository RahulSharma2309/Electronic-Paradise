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
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category,
            Brand = product.Brand,
            Unit = product.Unit,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive,
            HasCertification = product.Certification != null,
            CertificationType = product.Certification?.CertificationType,
        };
    }

    /// <inheritdoc/>
    public ProductDetailResponse ToDetailResponse(Product product)
    {
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
                Attributes = product.Metadata.GetAttributes(),
                Tags = product.Metadata.Tags,
                Slug = product.Metadata.Slug,
                SeoMetadata = seoMetadata,
            };
        }

        return new ProductDetailResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category,
            Brand = product.Brand,
            Sku = product.Sku,
            Unit = product.Unit,
            ImageUrl = product.ImageUrl,
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
            Category = request.Category,
            Brand = request.Brand,
            Sku = request.Sku,
            Unit = request.Unit,
            ImageUrl = request.ImageUrl,
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

        // Map metadata if provided
        if (request.Metadata != null)
        {
            product.Metadata = new ProductMetadata
            {
                ProductId = product.Id,
                Tags = request.Metadata.Tags,
                Slug = request.Metadata.Slug,
                CreatedAt = DateTime.UtcNow,
            };

            // Set attributes if provided
            if (request.Metadata.Attributes != null && request.Metadata.Attributes.Count > 0)
            {
                product.Metadata.SetAttributes(request.Metadata.Attributes);
            }

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

        if (request.Category != null)
        {
            product.Category = request.Category;
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
            product.ImageUrl = request.ImageUrl;
        }

        if (request.IsActive.HasValue)
        {
            product.IsActive = request.IsActive.Value;
        }

        product.UpdatedAt = DateTime.UtcNow;
    }
}
