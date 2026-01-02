using ProductService.Abstraction.Models;

namespace ProductService.Core.Business;

public interface IProductService
{
    Task<List<Product>> ListAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task CreateAsync(Product p);
    Task<int> ReserveAsync(Guid id, int quantity);
    Task<int> ReleaseAsync(Guid id, int quantity);
}




