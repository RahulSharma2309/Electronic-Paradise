using ProductService.Abstraction.Models;
using ProductService.Core.Repository;

namespace ProductService.Core.Business;

public class ProductServiceImpl : IProductService
{
    private readonly IProductRepository _repo;
    public ProductServiceImpl(IProductRepository repo) => _repo = repo;

    public Task<List<Product>> ListAsync() => _repo.GetAllAsync();

    public Task<Product?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public Task CreateAsync(Product p)
    {
        ValidateForCreate(p);
        return _repo.AddAsync(p);
    }

    public Task<int> ReserveAsync(Guid id, int quantity) => _repo.ReserveAsync(id, quantity);
    
    public Task<int> ReleaseAsync(Guid id, int quantity) => _repo.ReleaseAsync(id, quantity);

    private static void ValidateForCreate(Product p)
    {
        if (string.IsNullOrWhiteSpace(p.Name)) throw new ArgumentException("Name is required");
        if (p.Price < 0) throw new ArgumentException("Price must be >= 0");
        if (p.Stock < 0) throw new ArgumentException("Stock must be >= 0");
    }
}





