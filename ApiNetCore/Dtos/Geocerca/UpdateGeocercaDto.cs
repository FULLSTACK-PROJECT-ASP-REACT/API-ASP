namespace ApiNetCore.Dtos.Geocerca;

public class UpdateGeocercaDto
{
    public string Nombre { get; set; } = null!;
    public string Codigo { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public string DireccionReferencia { get; set; } = null!;
    public string Ciudad { get; set; } = null!;
    public string Provincia { get; set; } = null!;
    public string Pais { get; set; } = null!;
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string PoligonoCoordenadas { get; set; } = null!;
    public decimal? AreaMetrosCuadrados { get; set; }
    public decimal? PerimetroMetros { get; set; }
    public string? Estado { get; set; }
    public bool? Activa { get; set; }
    public string? ColorMapa { get; set; }
    public int Prioridad { get; set; }
    public string? Descripcion { get; set; }
    public string TipoArea { get; set; } = null!;
}