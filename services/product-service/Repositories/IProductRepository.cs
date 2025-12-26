using ProductService.Models;

namespace ProductService.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product p);
    Task<int> ReserveAsync(Guid id, int quantity);
    Task<int> ReleaseAsync(Guid id, int quantity);
}
