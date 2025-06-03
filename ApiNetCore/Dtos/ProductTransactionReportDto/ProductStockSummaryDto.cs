namespace ApiNetCore.Dtos.ProductTransactionReportDto;

public class ProductStockSummaryDto
{
    public int CurrentStock { get; set; }
    public int TotalPurchased { get; set; }
    public int TotalSold { get; set; }
    public decimal TotalPurchaseValue { get; set; }
    public decimal TotalSaleValue { get; set; }
    public decimal AveragePurchasePrice { get; set; }
    public decimal AverageSalePrice { get; set; }
    public int TotalTransactions { get; set; }
    public DateTime? FirstTransactionDate { get; set; }
    public DateTime? LastTransactionDate { get; set; }
}