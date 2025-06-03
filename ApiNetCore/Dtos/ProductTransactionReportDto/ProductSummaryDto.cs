namespace ApiNetCore.Dtos.ProductTransactionReportDto;

public class ProductSummaryDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal CurrentPrice { get; set; }
    public int CurrentStock { get; set; }
    public string? Status { get; set; }
    public string? Image { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}