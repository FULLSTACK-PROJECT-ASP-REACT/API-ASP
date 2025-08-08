using ApiNetCore.Dtos.Paginacion;

namespace ApiNetCore.Dtos.Vendedor.Listas;

public class LVendedorDto
{
    public List<VendedorDto> Vendedores { get; set; } = [];
    public PaginacionDto Paginacion { get; set; }
    public DateTime FechaConsulta { get; set; }
}