namespace ApiNetCore.Dtos.ProductTransactionReportDto;

public class MultipleProductsReportDto
{
    public List<ProductTransactionReportDto> Products { get; set; } = new();
    public int TotalProducts { get; set; }
    public DateTime ReportGeneratedAt { get; set; }
}