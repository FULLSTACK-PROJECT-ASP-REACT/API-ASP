using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Vendedor;
using ApiNetCore.Entities;
using AutoMapper;

namespace ApiNetCore.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateVendedorDto, Vendedor>();
        CreateMap<VendedorDto, Vendedor>().ReverseMap();
        CreateMap<Geocerca, GeocercaDto>().ReverseMap();
        CreateMap<Vendedor, VendedorDto>().ForMember(dest => dest.Geocercas, opt =>
                opt.MapFrom(src =>
                    src.TblGeoVens.Select(gv => gv.IdGeocercaNavigation).ToList()))
            .ReverseMap()
            .ForMember(dest => dest.TblGeoVens, opt => opt.Ignore());
        
        CreateMap<Vendedor, VendedorDto>()
            .ForMember(dest => dest.Geocercas, opt => opt.MapFrom(src => 
                src.TblGeoVens.Select(gv => gv.IdGeocercaNavigation).ToList()))
            .ReverseMap()
            .ForMember(dest => dest.TblGeoVens, opt => opt.Ignore());

        CreateMap<CreateGeocercaDto, Geocerca>().ReverseMap();
        
        CreateMap<CreateGeocercaVendedorDto, Geocerca>().ReverseMap();
        
        CreateMap<Vendedor, VendedorSinGeocercasDto>().ReverseMap();


        
        CreateMap<UpdateGeocercaDto, Geocerca>().ReverseMap();
        
        CreateMap<Geocerca, GeocercaDto>().ReverseMap();
        
        CreateMap<Geocerca, GeocercaConVendedorDto>()
            .ForMember(dest => dest.Vendedores, opt => opt.MapFrom(src => 
                src.TblGeoVens.Select(gv => gv.IdVendedorNavigation).ToList()))
            .ReverseMap()
            .ForMember(dest => dest.TblGeoVens, opt => opt.Ignore());
    }
}