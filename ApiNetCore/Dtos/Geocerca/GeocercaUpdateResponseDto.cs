namespace ApiNetCore.Dtos.Geocerca;

public class GeocercaUpdateResponseDto
{
    public string Geoccod { get; set; } = null!;
    public string Geocnom { get; set; } = null!;
    public DateTime FechaEdicion { get; set; }
    public string UsuarioEditor { get; set; } = null!;
    public string Mensaje { get; set; } = null!;
    public GeocercaDetailDto DetalleGeocerca { get; set; } = null!;
}