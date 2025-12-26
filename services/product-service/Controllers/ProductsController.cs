using Microsoft.AspNetCore.Mvc;
using ProductService.Services;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // POST: api/products/{id}/release
    // Body: { "quantity": 1 }
    [HttpPost("{id}/release")]
    public async Task<IActionResult> Release(Guid id, [FromBody] ProductService.Dtos.ReleaseDto dto)
    {
        if (dto.Quantity <= 0) return BadRequest(new { error = "Quantity must be > 0" });

        try
        {
            var remaining = await _service.ReleaseAsync(id, dto.Quantity);
            return Ok(new { id = id, remaining = remaining });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }


    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.ListAsync();
        var dto = products.Select(p => new { p.Id, p.Name, p.Description, p.Price, p.Stock });
        return Ok(dto);
    }

    // POST: api/products
    // Creates a new product with validation
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductService.Dtos.CreateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = new ProductService.Models.Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            // validate domain rules
            ProductValidator.ValidateForCreate(product);
            await _service.CreateAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, new { product.Id, product.Name, product.Description, product.Price, product.Stock });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var p = await _service.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(new { p.Id, p.Name, p.Description, p.Price, p.Stock });
    }

    // POST: api/products/{id}/reserve
    // Body: { "quantity": 1 }
    [HttpPost("{id}/reserve")]
    public async Task<IActionResult> Reserve(Guid id, ReserveDto dto)
    {
        if (dto.Quantity <= 0) return BadRequest(new { error = "Quantity must be > 0" });

        try
        {
            var remaining = await _service.ReserveAsync(id, dto.Quantity);
            return Ok(new { id = id, remaining = remaining });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}

public record ReserveDto(int Quantity);
