namespace ApiNetCore.Dtos.TransactionDTOs;

public class UpdateTransactionFullDto
{
    public string? Status { get; set; } // "A", "I", "C"
    public string? Message { get; set; }
    public List<UpdateDetailTransactionDto> Details { get; set; } = new();
}