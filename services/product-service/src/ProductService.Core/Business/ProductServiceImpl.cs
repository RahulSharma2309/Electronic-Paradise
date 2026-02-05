using Microsoft.Extensions.Logging;
using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using ProductService.Abstraction.Models;
using ProductService.Core.Mappers;
using ProductService.Core.Repository;

namespace ProductService.Core.Business;

/// <summary>
/// Provides implementation for product-related business operations.
/// </summary>
public class ProductServiceImpl : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IProductMapper _mapper;
    private readonly ILogger<ProductServiceImpl> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductServiceImpl"/> class.
    /// </summary>
    /// <param name="repo">The product repository.</param>
    /// <param name="mapper">The product mapper.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if parameters are null.</exception>
    public ProductServiceImpl(IProductRepository repo, IProductMapper mapper, ILogger<ProductServiceImpl> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<List<ProductResponse>> ListAsync()
    {
        _logger.LogDebug("Fetching all products");
        try
        {
            var products = await _repo.GetAllAsync();
            _logger.LogInformation("Retrieved {ProductCount} products", products.Count);
            return products.Select(_mapper.ToResponse).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all products");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ProductDetailResponse?> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("Fetching product {ProductId}", id);
        try
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogDebug("Product {ProductId} not found", id);
                return null;
            }

            _logger.LogDebug("Product {ProductId} found: {ProductName}, Stock: {Stock}", id, product.Name, product.Stock);
            return _mapper.ToDetailResponse(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product {ProductId}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ProductDetailResponse> CreateAsync(CreateProductRequest request)
    {
        _logger.LogInformation("Creating new product: {ProductName}, Price: {Price}, Stock: {Stock}", request.Name, request.Price, request.Stock);
        try
        {
            var product = _mapper.ToEntity(request);
            ValidateForCreate(product);

            // Normalize taxonomy: resolve Category (string request -> Category row -> FK)
            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                product.Category = await _repo.GetOrCreateCategoryAsync(request.Category);
            }

            // Normalize tags: resolve tags and create join rows (request.Metadata.Tags)
            if (request.Metadata?.Tags != null && request.Metadata.Tags.Length > 0)
            {
                var tags = await _repo.GetOrCreateTagsAsync(request.Metadata.Tags);
                foreach (var tag in tags)
                {
                    product.ProductTags.Add(new ProductTag
                    {
                        ProductId = product.Id,
                        TagId = tag.Id,
                        Tag = tag,
                    });
                }
            }

            await _repo.AddAsync(product);
            _logger.LogInformation("Product created successfully: {ProductId}, Name: {ProductName}", product.Id, product.Name);
            return _mapper.ToDetailResponse(product);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Product creation validation failed for product: {ProductName}", request.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product: {ProductName}", request.Name);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<int> ReserveAsync(Guid id, int quantity)
    {
        _logger.LogInformation("Reserving {Quantity} units of product {ProductId}", quantity, id);
        try
        {
            var newStock = await _repo.ReserveAsync(id, quantity);
            _logger.LogInformation("Successfully reserved {Quantity} units of product {ProductId}. New stock: {NewStock}", quantity, id, newStock);
            return newStock;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Stock reservation failed for product {ProductId}: Invalid quantity {Quantity}", id, quantity);
            throw;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Stock reservation failed: Product {ProductId} not found", id);
            throw;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Stock reservation failed for product {ProductId}: Insufficient stock for quantity {Quantity}", id, quantity);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reserving {Quantity} units of product {ProductId}", quantity, id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<int> ReleaseAsync(Guid id, int quantity)
    {
        _logger.LogInformation("Releasing {Quantity} units of product {ProductId}", quantity, id);
        try
        {
            var newStock = await _repo.ReleaseAsync(id, quantity);
            _logger.LogInformation("Successfully released {Quantity} units of product {ProductId}. New stock: {NewStock}", quantity, id, newStock);
            return newStock;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Stock release failed for product {ProductId}: Invalid quantity {Quantity}", id, quantity);
            throw;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Stock release failed: Product {ProductId} not found", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing {Quantity} units of product {ProductId}", quantity, id);
            throw;
        }
    }

    /// <summary>
    /// Validates a product before creation.
    /// </summary>
    /// <param name="p">The product to validate.</param>
    /// <exception cref="ArgumentException">Thrown if validation fails.</exception>
    private static void ValidateForCreate(Product p)
    {
        if (string.IsNullOrWhiteSpace(p.Name))
        {
            throw new ArgumentException("Name is required", nameof(p.Name));
        }

        if (p.Price < 0)
        {
            throw new ArgumentException("Price must be >= 0", nameof(p.Price));
        }

        if (p.Stock < 0)
        {
            throw new ArgumentException("Stock must be >= 0", nameof(p.Stock));
        }
    }
}
