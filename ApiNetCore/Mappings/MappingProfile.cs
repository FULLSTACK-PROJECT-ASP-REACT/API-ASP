using ApiNetCore.Dtos;
using ApiNetCore.Dtos.Vendedor;
using ApiNetCore.Entities;
using AutoMapper;

namespace ApiNetCore.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateVendedorDto, GeocercaVendedor>();
        CreateMap<VendedorDto, GeocercaVendedor>().ReverseMap();
    }
}