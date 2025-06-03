namespace ApiNetCore.Dtos.TransactionDTOs;

public class DetailTransactionDto
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public string? CodeStub { get; set; }
    public decimal PriceUnit { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
}