using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Geocerca.GeoUsu;
using ApiNetCore.Dtos.Paginacion;
using ApiNetCore.Dtos.Vendedor;

namespace ApiNetCore.Services.Interfaces;

public interface IGeocercaService
{
    Task<PaginatedResultDto<GeocercaListDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? estado = null, bool? activo = null);
    Task<PaginatedResultDto<GeocercaConVendedorDto>> GetAllGeocercaConVendedorAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? estado = null, bool? activo = null, bool soloConVendedores = false);
    Task<GeocercaDetailDto> GetByCodigoAsync(string codigo);
    Task<bool> ExistsAsync(string codigo);
    Task<GeocercaConVendedoresCreateResponseDto> CreateGeocercaConVendedoresAsync(GeocercaConVendedoresCreateDto createDto);
    


}

