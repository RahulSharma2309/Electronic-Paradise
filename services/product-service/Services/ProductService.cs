using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Services;

public class ProductServiceImpl : IProductService
{
    private readonly IProductRepository _repo;
    public ProductServiceImpl(IProductRepository repo) => _repo = repo;

    public Task<List<Product>> ListAsync() => _repo.GetAllAsync();

    public Task<Product?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public Task CreateAsync(Product p) => _repo.AddAsync(p);

    public Task<int> ReserveAsync(Guid id, int quantity) => _repo.ReserveAsync(id, quantity);
    public Task<int> ReleaseAsync(Guid id, int quantity) => _repo.ReleaseAsync(id, quantity);
}

// Add validation-aware creation
public static class ProductValidator
{
    public static void ValidateForCreate(Product p)
    {
        if (string.IsNullOrWhiteSpace(p.Name)) throw new ArgumentException("Name is required");
        if (p.Price < 0) throw new ArgumentException("Price must be >= 0");
        if (p.Stock < 0) throw new ArgumentException("Stock must be >= 0");
    }
}
