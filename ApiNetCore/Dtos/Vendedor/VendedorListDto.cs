namespace ApiNetCore.Dtos.Vendedor;

public class VendedorListDto
{
    
    public int Geugid { get; set; }
    public string Geugidv { get; set; } = null!;
    public string Geugidg { get; set; } = null!;
    public string Geuguscre { get; set; } = null!;
    public DateTime Geugfcre { get; set; }
    public string Geugeqcre { get; set; } = null!;
    public string Geugusedi { get; set; } = null!;
    public DateTime Geugfedi { get; set; }
    public string Geugeqedi { get; set; } = null!;
    public decimal Geuglat { get; set; }
    public decimal Geuglon { get; set; }
    
}