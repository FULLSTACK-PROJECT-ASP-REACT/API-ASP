namespace ApiNetCore.Dtos.TransactionDTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public DateTime? EmissionDate { get; set; }
    public string Type { get; set; }
    public string TypeDescription { get; set; } // "Compra" o "Venta"
    public int PriceUnit { get; set; }
    public int TotalAmount { get; set; }
    public string? Status { get; set; }
    public string? StatusDescription { get; set; } // "Activo", "Inactivo", etc.
    public string? Message { get; set; }
    public List<DetailTransactionDto> Details { get; set; } = new();
}
