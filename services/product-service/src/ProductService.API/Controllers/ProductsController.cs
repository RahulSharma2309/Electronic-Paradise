// -----------------------------------------------------------------------
// <copyright file="ProductsController.cs" company="FreshHarvest-Market">
// Copyright (c) FreshHarvest-Market. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using ProductService.Core.Business;

namespace ProductService.API.Controllers;

/// <summary>
/// Controller for managing products and stock operations.
/// FreshHarvest Market organic food marketplace API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsController"/> class.
    /// </summary>
    /// <param name="service">The product business service.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="service"/> or <paramref name="logger"/> is null.</exception>
    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> with a list of all products.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogDebug("Fetching all products");

        try
        {
            var products = await _service.ListAsync();
            _logger.LogInformation("Successfully retrieved {ProductCount} products", products.Count);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            return StatusCode(500, new { error = "Failed to retrieve products" });
        }
    }

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with product details if found,
    /// or <see cref="NotFoundResult"/> if not found.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogDebug("Fetching product {ProductId}", id);

        try
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved product {ProductId}", id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product {ProductId}", id);
            return StatusCode(500, new { error = "Failed to retrieve product" });
        }
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="request">The product creation request.</param>
    /// <returns>
    /// A <see cref="CreatedAtActionResult"/> with the created product if successful,
    /// or <see cref="BadRequestObjectResult"/> if validation fails.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        _logger.LogInformation("Creating new product: {ProductName}", request.Name);

        try
        {
            var product = await _service.CreateAsync(request);
            _logger.LogInformation("Product created successfully: {ProductId}, Name: {ProductName}", product.Id, product.Name);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Product creation failed: Validation error for product {ProductName}", request.Name);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product {ProductName}", request.Name);
            return StatusCode(500, new { error = "Failed to create product" });
        }
    }

    /// <summary>
    /// Reserves a specified quantity of stock for a product (decreases stock).
    /// Used by the Order service during checkout to hold inventory.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="dto">The quantity to reserve.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with remaining stock if successful,
    /// <see cref="BadRequestObjectResult"/> if quantity is invalid,
    /// <see cref="NotFoundResult"/> if product not found,
    /// or <see cref="ConflictObjectResult"/> if insufficient stock.
    /// </returns>
    [HttpPost("{id}/reserve")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Reserve(Guid id, [FromBody] ReserveStockRequest dto)
    {
        _logger.LogInformation("Reserving {Quantity} units of product {ProductId}", dto.Quantity, id);

        if (dto.Quantity <= 0)
        {
            _logger.LogWarning("Stock reservation failed for product {ProductId}: Invalid quantity {Quantity}", id, dto.Quantity);
            return BadRequest(new { error = "Quantity must be > 0" });
        }

        try
        {
            var remaining = await _service.ReserveAsync(id, dto.Quantity);
            _logger.LogInformation("Successfully reserved {Quantity} units of product {ProductId}. Remaining: {Remaining}", dto.Quantity, id, remaining);
            return Ok(new { id, remaining });
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Stock reservation failed: Product {ProductId} not found", id);
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Stock reservation failed for product {ProductId}: {ErrorMessage}", id, ex.Message);
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reserving {Quantity} units of product {ProductId}", dto.Quantity, id);
            return StatusCode(500, new { error = "Failed to reserve stock" });
        }
    }

    /// <summary>
    /// Releases a specified quantity of previously reserved stock (increases stock).
    /// Used by the Order service during rollback if checkout fails.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="dto">The quantity to release.</param>
    /// <returns>
    /// An <see cref="OkObjectResult"/> with remaining stock if successful,
    /// <see cref="BadRequestObjectResult"/> if quantity is invalid,
    /// or <see cref="NotFoundResult"/> if product not found.
    /// </returns>
    [HttpPost("{id}/release")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Release(Guid id, [FromBody] ReserveStockRequest dto)
    {
        _logger.LogInformation("Releasing {Quantity} units of product {ProductId}", dto.Quantity, id);

        if (dto.Quantity <= 0)
        {
            _logger.LogWarning("Stock release failed for product {ProductId}: Invalid quantity {Quantity}", id, dto.Quantity);
            return BadRequest(new { error = "Quantity must be > 0" });
        }

        try
        {
            var remaining = await _service.ReleaseAsync(id, dto.Quantity);
            _logger.LogInformation("Successfully released {Quantity} units of product {ProductId}. Remaining: {Remaining}", dto.Quantity, id, remaining);
            return Ok(new { id, remaining });
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Stock release failed: Product {ProductId} not found", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing {Quantity} units of product {ProductId}", dto.Quantity, id);
            return StatusCode(500, new { error = "Failed to release stock" });
        }
    }
}
