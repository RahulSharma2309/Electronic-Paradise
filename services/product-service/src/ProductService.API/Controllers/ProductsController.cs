using Microsoft.AspNetCore.Mvc;
using ProductService.Abstraction.DTOs;
using ProductService.Abstraction.Models;
using ProductService.Core.Business;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
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

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        try
        {
            await _service.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST: api/products/{id}/reserve
    [HttpPost("{id}/reserve")]
    public async Task<IActionResult> Reserve(Guid id, [FromBody] ReleaseDto dto)
    {
        if (dto.Quantity <= 0) return BadRequest(new { error = "Quantity must be > 0" });

        try
        {
            var remaining = await _service.ReserveAsync(id, dto.Quantity);
            return Ok(new { id, remaining });
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

    // POST: api/products/{id}/release
    [HttpPost("{id}/release")]
    public async Task<IActionResult> Release(Guid id, [FromBody] ReleaseDto dto)
    {
        if (dto.Quantity <= 0) return BadRequest(new { error = "Quantity must be > 0" });

        try
        {
            var remaining = await _service.ReleaseAsync(id, dto.Quantity);
            return Ok(new { id, remaining });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}





