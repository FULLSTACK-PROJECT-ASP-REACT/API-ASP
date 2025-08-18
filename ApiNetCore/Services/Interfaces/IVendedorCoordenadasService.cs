using ApiNetCore.Dtos.Usuario;

namespace ApiNetCore.Services.Interfaces;

public interface IVendedorCoordenadasService
{
    Task<List<VendedorCoordenadasDto>> GetListCoordenadasVendedorAsync(GetListCoordenadasVendedorRequestDto request);

}