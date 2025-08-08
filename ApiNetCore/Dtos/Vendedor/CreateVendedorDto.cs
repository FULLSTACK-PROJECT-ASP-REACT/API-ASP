namespace ApiNetCore.Dtos.Vendedor;

public class CreateVendedorDto
{
    public string? CodeVendedor { get; set; }
    public string? NombreVendedor { get; set; }
    public string? CoordenadasVendedor { get; set; }
    public decimal? LatitudVendedor { get; set; }
    public decimal? LongitudVendedor { get; set; }
    public string? ColorUbi { get; set; }
    public string? DireccionVendedor { get; set; }
    
}