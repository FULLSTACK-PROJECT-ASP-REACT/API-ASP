using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Dtos.Usuario;

public class GetListCoordenadasVendedorRequestDto
{
    [Required(ErrorMessage = "El token es requerido")]
    public string Token { get; set; } = null!;
    
    /// <summary>
    /// Filtrar solo usuarios con acceso a geolocalización (por defecto true)
    /// </summary>
    public bool? SoloConGeolocalizacion { get; set; } = true;
    
    /// <summary>
    /// Filtrar solo usuarios con acceso a la app (por defecto true)
    /// </summary>
    public bool? SoloConAccesoApp { get; set; } = true;
    
    /// <summary>
    /// Fecha desde para filtrar ubicaciones (opcional)
    /// </summary>
    public DateTime? FechaDesde { get; set; }
    
    /// <summary>
    /// Fecha hasta para filtrar ubicaciones (opcional)
    /// </summary>
    public DateTime? FechaHasta { get; set; }
}