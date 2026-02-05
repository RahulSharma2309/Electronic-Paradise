using Microsoft.EntityFrameworkCore;
using ProductService.Abstraction.Models;

namespace ProductService.Core.Data;

/// <summary>
/// Database context for the Product service.
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
    /// Gets or sets the product attributes table.
    /// </summary>
    public DbSet<ProductAttribute> ProductAttributes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tags table.
    /// </summary>
    public DbSet<Tag> Tags { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product-tags join table.
    /// </summary>
    public DbSet<ProductTag> ProductTags { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product certifications table.
    /// </summary>
    public DbSet<ProductCertification> ProductCertifications { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product metadata table (SEO/slug).
    /// </summary>
    public DbSet<ProductMetadata> ProductMetadata { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(200).IsRequired();
            entity.HasIndex(x => x.Slug).IsUnique();

            entity.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000);
            entity.Property(x => x.Brand).HasMaxLength(100);
            entity.Property(x => x.Sku).HasMaxLength(100);
            entity.Property(x => x.Unit).HasMaxLength(50);

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Url).HasMaxLength(500).IsRequired();
            entity.Property(x => x.AltText).HasMaxLength(250);
            entity.HasIndex(x => new { x.ProductId, x.SortOrder });

            entity.HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.ProductId, x.IsPrimary })
                .HasFilter("[IsPrimary] = 1")
                .IsUnique();
        });

        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Key).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Group).HasMaxLength(100);
            entity.Property(x => x.Unit).HasMaxLength(50);
            entity.Property(x => x.ValueString).HasMaxLength(4000);
            entity.Property(x => x.ValueNumber).HasPrecision(18, 4);
            entity.HasIndex(x => new { x.ProductId, x.Key });

            entity.HasOne(x => x.Product)
                .WithMany(x => x.Attributes)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(120).IsRequired();
            entity.HasIndex(x => x.Slug).IsUnique();
        });

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

        modelBuilder.Entity<ProductCertification>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CertificationNumber).HasMaxLength(100).IsRequired();
            entity.Property(x => x.CertificationType).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Origin).HasMaxLength(200);
            entity.Property(x => x.CertifyingAgency).HasMaxLength(200);
            entity.Property(x => x.Notes).HasMaxLength(1000);
            entity.HasIndex(x => x.ProductId).IsUnique();

            entity.HasOne(x => x.Product)
                .WithOne(x => x.Certification)
                .HasForeignKey<ProductCertification>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductMetadata>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Slug).HasMaxLength(200);
            entity.Property(x => x.SeoMetadataJson).HasMaxLength(4000);
            entity.HasIndex(x => x.ProductId).IsUnique();
            entity.HasIndex(x => x.Slug).IsUnique().HasFilter("[Slug] IS NOT NULL");

            entity.HasOne(x => x.Product)
                .WithOne(x => x.Metadata)
                .HasForeignKey<ProductMetadata>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
