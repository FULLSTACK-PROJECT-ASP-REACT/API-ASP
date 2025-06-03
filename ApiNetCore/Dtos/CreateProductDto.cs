using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Dtos;

public class CreateProductDto
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public string Code { get; set; }
    public int Stock { get; set; } = 0;
    public IFormFile? Image { get; set; } 
    public List<int> CategoryIds { get; set; } = [];
}