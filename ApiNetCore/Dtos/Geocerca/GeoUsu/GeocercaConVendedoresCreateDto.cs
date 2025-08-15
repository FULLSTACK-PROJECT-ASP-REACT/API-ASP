using System.ComponentModel.DataAnnotations;
using ApiNetCore.Dtos.Vendedor;

namespace ApiNetCore.Dtos.Geocerca.GeoUsu;

public class GeocercaConVendedoresCreateDto
{
    [Required(ErrorMessage = "El código de geocerca es requerido")]
    [StringLength(10, ErrorMessage = "El código no puede exceder 10 caracteres")]
    public string Geoccod { get; set; } = null!;

    [Required(ErrorMessage = "El nombre de geocerca es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Geocnom { get; set; } = null!;

    [Required(ErrorMessage = "El sector es requerido")]
    [StringLength(50, ErrorMessage = "El sector no puede exceder 50 caracteres")]
    public string Geocsec { get; set; } = null!;

    [Required(ErrorMessage = "La dirección de referencia es requerida")]
    [StringLength(100, ErrorMessage = "La dirección no puede exceder 100 caracteres")]
    public string Geocdirre { get; set; } = null!;

    [Required(ErrorMessage = "La ciudad es requerida")]
    [StringLength(30, ErrorMessage = "La ciudad no puede exceder 30 caracteres")]
    public string Geocciud { get; set; } = null!;

    [Required(ErrorMessage = "La provincia es requerida")]
    [StringLength(30, ErrorMessage = "La provincia no puede exceder 30 caracteres")]
    public string Geocprov { get; set; } = null!;

    [Required(ErrorMessage = "El país es requerido")]
    [StringLength(30, ErrorMessage = "El país no puede exceder 30 caracteres")]
    public string Geocpais { get; set; } = null!;

    [Required(ErrorMessage = "La latitud es requerida")]
    [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90")]
    public decimal Geoclat { get; set; }

    [Required(ErrorMessage = "La longitud es requerida")]
    [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180")]
    public decimal Geoclon { get; set; }

    public object? Geoccoor { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El área debe ser mayor a 0")]
    public decimal Geocarm { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El perímetro debe ser mayor a 0")]
    public decimal Geocperm { get; set; }

    [Required(ErrorMessage = "El estado de geocerca es requerido")]
    [StringLength(1, ErrorMessage = "El estado debe ser un solo carácter")]
    public string Geocest { get; set; } = null!;

    public bool? Geocact { get; set; } = true;

    [Range(0, int.MaxValue, ErrorMessage = "La prioridad debe ser mayor o igual a 0")]
    public int Geocpri { get; set; }

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(250, ErrorMessage = "La descripción no puede exceder 250 caracteres")]
    public string Geocdesc { get; set; } = null!;

    [Required(ErrorMessage = "El usuario creador es requerido")]
    [StringLength(10, ErrorMessage = "El usuario creador no puede exceder 10 caracteres")]
    public string Geocuscre { get; set; } = null!;

    [Required(ErrorMessage = "El equipo creador es requerido")]
    [StringLength(50, ErrorMessage = "El equipo creador no puede exceder 50 caracteres")]
    public string Geoceqcre { get; set; } = null!;

    /// <summary>
    /// Lista de vendedores a asignar a esta geocerca
    /// </summary>
    public List<VendedorCreateDto> Vendedores { get; set; } = new();

    // === VALIDACIONES ADICIONALES ===
    /// <summary>
    /// Si es true, validará que no existan duplicados de vendedores en la lista
    /// </summary>
    public bool ValidarVendedoresDuplicados { get; set; } = true;
    
}