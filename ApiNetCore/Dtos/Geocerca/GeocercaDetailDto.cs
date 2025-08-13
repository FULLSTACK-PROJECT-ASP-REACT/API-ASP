namespace ApiNetCore.Dtos.Geocerca;

public class GeocercaDetailDto
{
    public string Geoccod { get; set; } = null!;
    public string Geocnom { get; set; } = null!;
    public string Geocsec { get; set; } = null!;
    public string Geocdirre { get; set; } = null!;
    public string Geocciud { get; set; } = null!;
    public string Geocprov { get; set; } = null!;
    public string Geocpais { get; set; } = null!;
    public decimal Geoclat { get; set; }
    public decimal Geoclon { get; set; }
    public object? Geoccoor { get; set; }
    public decimal Geocarm { get; set; }
    public decimal Geocperm { get; set; }
    public string Geocest { get; set; } = null!;
    public bool? Geocact { get; set; }
    public int Geocpri { get; set; }
    public string Geocdesc { get; set; } = null!;
    public string Geocuscre { get; set; } = null!;
    public DateTime Geocfcre { get; set; }
    public string Geoceqcre { get; set; } = null!;
    public string Geocusedi { get; set; } = null!;
    public DateTime Geocfedi { get; set; }
    public string Geoceqedi { get; set; } = null!;
}