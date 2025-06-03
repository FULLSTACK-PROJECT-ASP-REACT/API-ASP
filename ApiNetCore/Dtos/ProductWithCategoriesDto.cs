namespace ApiNetCore.Dtos;

public class ProductWithCategoriesDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }
    public int Stock { get; set; }
    public string? Code { get; set; }
    public string? Image { get; set; }
    public List<CategoryDto> Categories { get; set; } = [];
}