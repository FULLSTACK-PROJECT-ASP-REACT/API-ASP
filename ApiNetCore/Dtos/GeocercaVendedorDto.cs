namespace ApiNetCore.Dtos;

public class GeocercaVendedorDto
{
    public string Geoccod { get; set; } = string.Empty;
    public string Geocnom { get; set; } = string.Empty;
    public string Geocsec { get; set; } = string.Empty;
    public string Geocciud { get; set; } = string.Empty;
    public string Geocdirre { get; set; } = null!;

    public string Geocprov { get; set; } = string.Empty;
    public string Geocest { get; set; } = string.Empty;
    public bool? Geocact { get; set; }
    public int Geocpri { get; set; }
    public decimal Geoclat { get; set; }
    public decimal Geoclon { get; set; }
    
    public decimal Geocarm { get; set; }

    public decimal Geocperm { get; set; }

    public object? Geoccoor { get; set; }
    public DateTime FechaAsignacion { get; set; }
}