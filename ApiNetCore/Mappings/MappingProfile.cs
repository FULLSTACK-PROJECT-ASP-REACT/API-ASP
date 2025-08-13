using System.Text.Json;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Models;
using AutoMapper;

namespace ApiNetCore.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo de CreateDto a Model
        CreateMap<GeocercaCreateDto, Geogeoc>()
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => ConvertObjectToJson(src.Geoccoor)))
            .ForMember(dest => dest.Geocfcre, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geocfedi, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geocusedi, opt => opt.MapFrom(src => src.Geocuscre))
            .ForMember(dest => dest.Geoceqedi, opt => opt.MapFrom(src => src.Geoceqcre));

        // Mapeo de UpdateDto a Model
        CreateMap<GeocercaUpdateDto, Geogeoc>()
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => ConvertObjectToJson(src.Geoccoor)))
            .ForMember(dest => dest.Geocfedi, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geoccod, opt => opt.Ignore())
            .ForMember(dest => dest.Geocfcre, opt => opt.Ignore())
            .ForMember(dest => dest.Geocuscre, opt => opt.Ignore())
            .ForMember(dest => dest.Geoceqcre, opt => opt.Ignore());

        // Mapeo de Model a ListDto
        CreateMap<Geogeoc, GeocercaListDto>();

        // Mapeo de Model a DetailDto
        CreateMap<Geogeoc, GeocercaDetailDto>()
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => ConvertJsonToObject(src.Geoccoor)));
    }

    private static string? ConvertObjectToJson(object? coordinates)
    {
        if (coordinates == null)
            return null;

        try
        {
            return JsonSerializer.Serialize(coordinates);
        }
        catch
        {
            return null;
        }
    }

    private static object? ConvertJsonToObject(string? jsonCoordinates)
    {
        if (string.IsNullOrEmpty(jsonCoordinates))
            return null;

        try
        {
            return JsonSerializer.Deserialize<object>(jsonCoordinates);
        }
        catch
        {
            return null;
        }
    }
}