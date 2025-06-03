using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Dtos.TransactionDTOs;

public class CreateTransactionDto
{
    [Required]
    [StringLength(1)]
    public string Type { get; set; } = null!; // "C" = Compra, "V" = Venta

    public string? Message { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one detail item is required")]
    public List<CreateDetailTransactionDto> Details { get; set; } = new();
}