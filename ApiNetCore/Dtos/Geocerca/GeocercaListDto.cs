namespace ApiNetCore.Dtos.Geocerca;

public class GeocercaListDto
{
    public string Geoccod { get; set; } = null!;
    public string Geocnom { get; set; } = null!;
    public string Geocsec { get; set; } = null!;
    public string Geocciud { get; set; } = null!;
    public string Geocprov { get; set; } = null!;
    public string Geocpais { get; set; } = null!;
    public decimal Geoclat { get; set; }
    public decimal Geoclon { get; set; }
    public decimal Geocarm { get; set; }
    public string Geocest { get; set; } = null!;
    public bool? Geocact { get; set; }
    public int Geocpri { get; set; }
    public DateTime Geocfcre { get; set; }
}