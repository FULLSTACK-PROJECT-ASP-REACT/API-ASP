using ApiNetCore.Dtos;

namespace ApiNetCore.Services.Interfaces;

public interface IVendedorExternoService
{
    Task<List<VendedorExternoDto>> GetVendedoresAsync(string token);

}