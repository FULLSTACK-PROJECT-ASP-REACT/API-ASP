using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Dtos;

public class UpdateProductDto
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    public string? Code { get; set; }
    public int Stock { get; set; }
    public IFormFile? Image { get; set; } 
    public List<int> CategoryIds { get; set; } = [];
    
}