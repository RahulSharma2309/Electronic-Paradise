using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Abstraction.Models;
using ProductService.Core.Data;

namespace ProductService.Core.Repository;

/// <summary>
/// Provides data access operations for <see cref="Product"/> entities.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<ProductRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    /// <param name="db">The application database context.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="db"/> or <paramref name="logger"/> is null.</exception>
    public ProductRepository(AppDbContext db, ILogger<ProductRepository> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<List<Product>> GetAllAsync()
    {
        _logger.LogDebug("Fetching all products from database");
        try
        {
            var products = await _db.Products.ToListAsync();
            _logger.LogDebug("Retrieved {ProductCount} products from database", products.Count);
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all products from database");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("Fetching product {ProductId} from database", id);
        try
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogDebug("Product {ProductId} not found in database", id);
            }
            else
            {
                _logger.LogDebug("Product {ProductId} found in database: {ProductName}", id, product.Name);
            }

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product {ProductId} from database", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task AddAsync(Product p)
    {
        _logger.LogDebug("Adding product {ProductName} to database", p.Name);
        try
        {
            _db.Products.Add(p);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Product added to database successfully: {ProductId}, Name: {ProductName}", p.Id, p.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product {ProductName} to database", p.Name);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<int> ReserveAsync(Guid id, int quantity)
    {
        _logger.LogDebug("Reserving {Quantity} units of product {ProductId} in database transaction", quantity, id);

        if (quantity <= 0)
        {
            _logger.LogWarning("Stock reservation failed for product {ProductId}: Invalid quantity {Quantity}", id, quantity);
            throw new ArgumentException("Quantity must be > 0", nameof(quantity));
        }

        try
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? tx = null;
            if (_db.Database.IsRelational())
            {
                tx = await _db.Database.BeginTransactionAsync();
            }

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                _logger.LogWarning("Stock reservation failed: Product {ProductId} not found", id);
                throw new KeyNotFoundException("Product not found");
            }

            if (product.Stock < quantity)
            {
                _logger.LogWarning("Stock reservation failed for product {ProductId}: Insufficient stock. Available: {Stock}, Requested: {Quantity}", id, product.Stock, quantity);
                throw new InvalidOperationException("Insufficient stock");
            }

            product.Stock -= quantity;
            await _db.SaveChangesAsync();
            if (tx != null)
            {
                await tx.CommitAsync();
                await tx.DisposeAsync();
            }

            _logger.LogInformation("Successfully reserved {Quantity} units of product {ProductId}. New stock: {NewStock}", quantity, id, product.Stock);
            return product.Stock;
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
        {
            _logger.LogError(ex, "Error reserving {Quantity} units of product {ProductId}", quantity, id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<int> ReleaseAsync(Guid id, int quantity)
    {
        _logger.LogDebug("Releasing {Quantity} units of product {ProductId} in database transaction", quantity, id);

        if (quantity <= 0)
        {
            _logger.LogWarning("Stock release failed for product {ProductId}: Invalid quantity {Quantity}", id, quantity);
            throw new ArgumentException("Quantity must be > 0", nameof(quantity));
        }

        try
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? tx = null;
            if (_db.Database.IsRelational())
            {
                tx = await _db.Database.BeginTransactionAsync();
            }

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                _logger.LogWarning("Stock release failed: Product {ProductId} not found", id);
                throw new KeyNotFoundException("Product not found");
            }

            product.Stock += quantity;
            await _db.SaveChangesAsync();
            if (tx != null)
            {
                await tx.CommitAsync();
                await tx.DisposeAsync();
            }

            _logger.LogInformation("Successfully released {Quantity} units of product {ProductId}. New stock: {NewStock}", quantity, id, product.Stock);
            return product.Stock;
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException)
        {
            _logger.LogError(ex, "Error releasing {Quantity} units of product {ProductId}", quantity, id);
            throw;
        }
    }
}
