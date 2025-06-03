using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Dtos.TransactionDTOs;

public class UpdateTransactionDto
{
    [StringLength(1)]
    public string? Status { get; set; } // "A" = Activo, "I" = Inactivo, "C" = Cancelado

    public string? Message { get; set; }
}