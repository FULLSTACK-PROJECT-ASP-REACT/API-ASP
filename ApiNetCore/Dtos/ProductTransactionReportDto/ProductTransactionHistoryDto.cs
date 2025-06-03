namespace ApiNetCore.Dtos.ProductTransactionReportDto;

public class ProductTransactionHistoryDto
{
    public int TransactionId { get; set; }
    public string TransactionCode { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string TransactionType { get; set; }
    public string TransactionTypeDescription { get; set; }
    public string? TransactionStatus { get; set; }
    public string? TransactionStatusDescription { get; set; }
    public string? TransactionMessage { get; set; }
    
    // Detalles específicos del producto en esta transacción
    public int DetailId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public string? DetailDescription { get; set; }
    public string? DetailCode { get; set; }
    
    // Impacto en stock
    public string StockImpact { get; set; } // "+" para compras, "-" para ventas
    public int StockChange { get; set; }
    public int StockAfterTransaction { get; set; }
}