using System.ComponentModel.DataAnnotations;

namespace ProductService.Abstraction.DTOs;

public class CreateProductDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}





