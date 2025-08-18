namespace ApiNetCore.Dtos.Usuario;

public class VendedorCoordenadasDto
{
    public string CodigoUsuario { get; set; } = null!;
    public string NombreUsuario { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool TieneAccesoApp { get; set; }
    public bool TieneAccesoWebApp { get; set; }
    public bool TieneAccesoGeolocalizacion { get; set; }
    public List<UbicacionDto> Ubicaciones { get; set; } = new();
    public int TotalUbicaciones { get; set; }
    public DateTime? UltimaUbicacion { get; set; }
}