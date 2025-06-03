using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Dtos.TransactionDTOs;

public class AddDetailToTransactionDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Product ID must be greater than 0")]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public int Amount { get; set; }

    public string? Description { get; set; }
    public decimal? CustomPriceUnit { get; set; }
}