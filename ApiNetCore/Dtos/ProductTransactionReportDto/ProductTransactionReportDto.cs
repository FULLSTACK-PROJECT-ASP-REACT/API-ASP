namespace ApiNetCore.Dtos.ProductTransactionReportDto;

public class ProductTransactionReportDto
{
    public ProductSummaryDto Product { get; set; } = new();
    public List<ProductTransactionHistoryDto> TransactionHistory { get; set; } = new();
    public ProductStockSummaryDto StockSummary { get; set; } = new();
}