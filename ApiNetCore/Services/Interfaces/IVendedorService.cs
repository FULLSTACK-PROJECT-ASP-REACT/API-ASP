using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Entities;

namespace ApiNetCore.Services.Interfaces;

public interface IVendedorService
{
    Task<CreateVendedorDto> CreateVendedor(CreateVendedorDto createVendedorDto);
    
}