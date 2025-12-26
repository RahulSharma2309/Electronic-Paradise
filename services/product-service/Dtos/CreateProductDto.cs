using System.ComponentModel.DataAnnotations;

namespace ProductService.Dtos;

public class CreateProductDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [StringLength(2000)]
    public string? Description { get; set; }

    // Price in cents to avoid floating point
    [Range(0, int.MaxValue)]
    public int Price { get; set; }

    // Available stock count
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
