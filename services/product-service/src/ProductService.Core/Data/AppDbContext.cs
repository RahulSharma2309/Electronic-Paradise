// -----------------------------------------------------------------------
// <copyright file="AppDbContext.cs" company="FreshHarvest-Market">
// Copyright (c) FreshHarvest-Market. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using ProductService.Abstraction.Models;

namespace ProductService.Core.Data;

/// <summary>
/// Database context for the FreshHarvest Market Product service.
/// Simplified schema optimized for organic food marketplace.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the products table.
    /// </summary>
    public DbSet<Product> Products { get; set; } = null!;

    /// <summary>
    /// Gets or sets the categories table.
    /// </summary>
    public DbSet<Category> Categories { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product images table.
    /// </summary>
    public DbSet<ProductImage> ProductImages { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tags table.
    /// </summary>
    public DbSet<Tag> Tags { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product-tags join table.
    /// </summary>
    public DbSet<ProductTag> ProductTags { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============================================================================
        // Category Configuration
        // ============================================================================
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(1000);
            entity.HasIndex(x => x.Slug).IsUnique();

            // Self-referencing hierarchy for subcategories
            entity.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ============================================================================
        // Product Configuration (Core entity with organic attributes)
        // ============================================================================
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);

            // Core fields
            entity.Property(x => x.Name).HasMaxLength(300).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(300).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(4000);
            entity.Property(x => x.Unit).HasMaxLength(50);
            entity.Property(x => x.Sku).HasMaxLength(100);
            entity.Property(x => x.Brand).HasMaxLength(150);

            // Organic & Freshness fields
            entity.Property(x => x.Origin).HasMaxLength(200);
            entity.Property(x => x.FarmName).HasMaxLength(200);

            // Certification fields (inline instead of separate table)
            entity.Property(x => x.CertificationNumber).HasMaxLength(100);
            entity.Property(x => x.CertificationType).HasMaxLength(100);
            entity.Property(x => x.CertifyingAgency).HasMaxLength(200);

            // Nutrition JSON (flexible schema within structured DB)
            entity.Property(x => x.NutritionJson).HasMaxLength(4000);

            // SEO fields (inline instead of separate table)
            entity.Property(x => x.SeoTitle).HasMaxLength(200);
            entity.Property(x => x.SeoDescription).HasMaxLength(500);

            // Indexes for common queries
            entity.HasIndex(x => x.Slug).IsUnique();
            entity.HasIndex(x => x.Sku).IsUnique().HasFilter("[Sku] IS NOT NULL");
            entity.HasIndex(x => x.IsActive);
            entity.HasIndex(x => x.IsFeatured);
            entity.HasIndex(x => x.IsOrganic);
            entity.HasIndex(x => x.CategoryId);
            entity.HasIndex(x => x.BestBefore);

            // Category relationship
            entity.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ============================================================================
        // ProductImage Configuration
        // ============================================================================
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Url).HasMaxLength(500).IsRequired();
            entity.Property(x => x.AltText).HasMaxLength(250);

            entity.HasIndex(x => new { x.ProductId, x.SortOrder });

            // Only one primary image per product
            entity.HasIndex(x => new { x.ProductId, x.IsPrimary })
                .HasFilter("[IsPrimary] = 1")
                .IsUnique();

            entity.HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ============================================================================
        // Tag Configuration
        // ============================================================================
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(120).IsRequired();
            entity.HasIndex(x => x.Slug).IsUnique();
        });

        // ============================================================================
        // ProductTag Configuration (Many-to-Many join)
        // ============================================================================
        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.HasKey(x => new { x.ProductId, x.TagId });

            entity.HasOne(x => x.Product)
                .WithMany(x => x.ProductTags)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Tag)
                .WithMany(x => x.ProductTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
