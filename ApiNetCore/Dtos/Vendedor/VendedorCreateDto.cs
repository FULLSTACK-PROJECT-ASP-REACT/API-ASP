using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Dtos.Vendedor;

public class VendedorCreateDto
{
    [Required(ErrorMessage = "El ID del vendedor es requerido")]
    [StringLength(10, ErrorMessage = "El ID del vendedor no puede exceder 10 caracteres")]
    public string Geugidv { get; set; } = null!;

    [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90")]
    public decimal Geuglat { get; set; }

    [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180")]
    public decimal Geuglon { get; set; }

    [Required(ErrorMessage = "El usuario creador es requerido")]
    [StringLength(10, ErrorMessage = "El usuario creador no puede exceder 10 caracteres")]
    public string Geuguscre { get; set; } = null!;

    [Required(ErrorMessage = "El equipo creador es requerido")]
    [StringLength(50, ErrorMessage = "El equipo creador no puede exceder 50 caracteres")]
    public string Geugeqcre { get; set; } = null!;
}