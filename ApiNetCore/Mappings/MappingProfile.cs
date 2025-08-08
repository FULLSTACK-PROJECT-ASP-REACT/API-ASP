using ApiNetCore.Dtos;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Entities;
using AutoMapper;

namespace ApiNetCore.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateVendedorDto, GeocercaVendedor>()    
            .ForMember(dest => dest.NombreVendedor, opt => opt.MapFrom(src => src.NombreVendedor))
            .ForMember(dest => dest.CodeVendedor, opt => opt.MapFrom(src => src.CodeVendedor))
            .ForMember(dest => dest.CoordenadasVendedor, opt => opt.MapFrom(src => src.CoordenadasVendedor))
            .ForMember(dest => dest.LongitudVendedor, opt => opt.MapFrom(src => src.LongitudVendedor))
            .ForMember(dest => dest.LatitudVendedor, opt => opt.MapFrom(src => src.LatitudVendedor))
            .ForMember(dest => dest.ColorUbi, opt => opt.MapFrom(src => src.ColorUbicacion))
            .ForMember(dest => dest.DireccionVendedor, opt => opt.MapFrom(src => src.DireccionVendedor));
    }
}