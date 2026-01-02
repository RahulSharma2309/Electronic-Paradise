using Microsoft.EntityFrameworkCore;
using ProductService.Abstraction.Models;
using ProductService.Core.Data;

namespace ProductService.Core.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public async Task<List<Product>> GetAllAsync() => await _db.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id) => await _db.Products.FindAsync(id);

    public async Task AddAsync(Product p)
    {
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
    }

    public async Task<int> ReserveAsync(Guid id, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be > 0", nameof(quantity));

        using var tx = await _db.Database.BeginTransactionAsync();
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) throw new KeyNotFoundException("Product not found");
        if (product.Stock < quantity) throw new InvalidOperationException("Insufficient stock");

        product.Stock -= quantity;
        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return product.Stock;
    }

    public async Task<int> ReleaseAsync(Guid id, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be > 0", nameof(quantity));

        using var tx = await _db.Database.BeginTransactionAsync();
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) throw new KeyNotFoundException("Product not found");

        product.Stock += quantity;
        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return product.Stock;
    }
}




