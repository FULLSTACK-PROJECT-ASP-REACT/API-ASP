using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Dtos.TransactionDTOs;

public class UpdateDetailTransactionDto
{
    [Required]
    public int DetailId { get; set; } // ID del detalle existente
    
    [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public int Amount { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Price unit must be greater than 0")]
    public decimal PriceUnit { get; set; }
    
    public string? Description { get; set; }
    
    // Opcional: permitir cambiar el producto (con validaciones adicionales)
    public int? NewProductId { get; set; }
}