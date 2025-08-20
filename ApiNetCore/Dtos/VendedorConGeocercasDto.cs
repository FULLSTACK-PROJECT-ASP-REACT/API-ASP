namespace ApiNetCore.Dtos;

public class VendedorConGeocercasDto
{
    public string CodigoVendedor { get; set; } = string.Empty;
    public string NombreVendedor { get; set; } = string.Empty;
    public string EmailVendedor { get; set; } = string.Empty;
    public string CodigoVendedorSecundario { get; set; } = string.Empty;
    public UbicacionVendedorDto? UbicacionActual { get; set; }
    public List<GeocercaVendedorDto> Geocercas { get; set; } = new();
    public int TotalGeocercas { get; set; }
}