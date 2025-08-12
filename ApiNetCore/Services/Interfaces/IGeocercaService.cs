using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Vendedor.Listas;

namespace ApiNetCore.Services.Interfaces;

public interface IGeocercaService
{
    Task<CreateGeocercaDto> CreateGeocerca(CreateGeocercaDto createGeocercaDto);
    Task<UpdateGeocercaDto> UpdateGeocerca(int idGeocerca, UpdateGeocercaDto updateGeocercaDto);
    Task<bool> DeleteGeocerca(int idGeocerca);
    Task<LGeocercaDto> GetGeocercasAsync(int pagina = 1, int tamanioPagina = 10);
    Task<GeocercaConVendedorDto> GetGeocercaByIdAsync(int idGeocerca);
    Task<CreateGeocercaVendedorDto> CreateGeocercaForVendedor(int idVendedor, CreateGeocercaVendedorDto createGeocercaVendedorDto);

}