using ApiNetCore.Dtos.Vendedor;
using ApiNetCore.Dtos.Vendedor.Listas;
using ApiNetCore.Entities;

namespace ApiNetCore.Services.Interfaces;

public interface IVendedorService
{
    Task<CreateVendedorDto> CreateVendedor(CreateVendedorDto createVendedorDto);
    Task<LVendedorDto> GetVendedoresAsync(int pagina = 1, int tamanioPagina = 10); 
    
}