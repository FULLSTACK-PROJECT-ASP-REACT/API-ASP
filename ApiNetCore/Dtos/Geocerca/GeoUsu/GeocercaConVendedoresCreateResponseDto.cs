namespace ApiNetCore.Dtos.Geocerca.GeoUsu;

public class GeocercaConVendedoresCreateResponseDto
{
    public string Geoccod { get; set; } = null!;
    public string Geocnom { get; set; } = null!;
    public string Mensaje { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
    public int TotalVendedoresCreados { get; set; }
    public List<string> VendedoresCreados { get; set; } = [];
    public List<string>? ErroresVendedores { get; set; }
    public GeocercaDetailDto? DetalleGeocerca { get; set; }
}