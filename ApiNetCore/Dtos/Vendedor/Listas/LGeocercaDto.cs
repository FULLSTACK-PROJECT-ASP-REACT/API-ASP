using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Paginacion;

namespace ApiNetCore.Dtos.Vendedor.Listas;

public class LGeocercaDto
{
    public List<GeocercaConVendedorDto> Geocercas { get; set; } = [];
    public PaginacionDto Paginacion { get; set; }
    public DateTime FechaConsulta { get; set; }
}