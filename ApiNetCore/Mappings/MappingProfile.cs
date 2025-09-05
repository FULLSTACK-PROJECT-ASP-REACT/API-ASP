using System.Text.Json;
using ApiNetCore.Dtos;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Geocerca.GeoUsu;
using ApiNetCore.Dtos.Vendedor;
using ApiNetCore.Models;
using AutoMapper;

namespace ApiNetCore.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo de VendorGeofenceAssignmentDto a Geogyu
        CreateMap<VendorGeofenceAssignmentDto, Geogyu>()
            .ForMember(dest => dest.Geugid, opt => opt.Ignore()) // Auto-generado
            .ForMember(dest => dest.Geugfcre, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geugfedi, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geugusedi, opt => opt.MapFrom(src => src.Geuguscre))
            .ForMember(dest => dest.Geugeqedi, opt => opt.MapFrom(src => src.Geugeqcre));

// Mapeo de Geogyu a VendorGeofenceAssignmentDto (para el retorno)
        CreateMap<Geogyu, VendorGeofenceAssignmentDto>()
            .ForMember(dest => dest.Geuguscre, opt => opt.MapFrom(src => src.Geuguscre))
            .ForMember(dest => dest.Geugeqcre, opt => opt.MapFrom(src => src.Geugeqcre));
        
        // Mapear desde Geogyu + Geogeoc a GeocercaVendedorDto
        CreateMap<Geogyu, GeocercaVendedorDto>()
            .ForMember(dest => dest.Geoccod, opt => opt.MapFrom(src => src.GeugidgNavigation.Geoccod))
            .ForMember(dest => dest.Geocnom, opt => opt.MapFrom(src => src.GeugidgNavigation.Geocnom))
            .ForMember(dest => dest.Geocsec, opt => opt.MapFrom(src => src.GeugidgNavigation.Geocsec))
            .ForMember(dest => dest.Geocciud, opt => opt.MapFrom(src => src.GeugidgNavigation.Geocciud))
            .ForMember(dest => dest.Geocdirre, opt => opt.MapFrom(src => src.GeugidgNavigation.Geocdirre))
            .ForMember(dest => dest.Geocprov, opt => opt.MapFrom(src => src.GeugidgNavigation.Geocprov))
            .ForMember(dest => dest.Geocest, opt => opt.MapFrom(src => src.GeugidgNavigation.Geocest))
            .ForMember(dest => dest.Geocact, opt => opt.MapFrom(src => src.GeugidgNavigation.Geocact))
            .ForMember(dest => dest.Geocpri, opt => opt.MapFrom(src => src.GeugidgNavigation.Geocpri))
            .ForMember(dest => dest.Geoclat, opt => opt.MapFrom(src => src.GeugidgNavigation.Geoclat))
            .ForMember(dest => dest.Geoclon, opt => opt.MapFrom(src => src.GeugidgNavigation.Geoclon))
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => src.GeugidgNavigation.Geoccoor))
            .ForMember(dest => dest.FechaAsignacion, opt => opt.MapFrom(src => src.Geugfcre));
        
        // Mapear desde GeocercaCreateDto a Geogeoc
        CreateMap<GeocercaCreateDto, Geogeoc>()
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => ConvertObjectToJson(src.Geoccoor)))
            .ForMember(dest => dest.Geocfcre, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geocfedi, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geocusedi, opt => opt.MapFrom(src => src.Geocuscre))
            .ForMember(dest => dest.Geoceqedi, opt => opt.MapFrom(src => src.Geoceqcre));
        
        
        
        // Para actualización de geocerca
        CreateMap<GeocercaUpdateDto, Geogeoc>()
            .ForMember(dest => dest.Geoccod, opt => opt.Ignore()) // No se actualiza el código
            .ForMember(dest => dest.Geocfcre, opt => opt.Ignore()) // No se actualiza fecha creación
            .ForMember(dest => dest.Geocuscre, opt => opt.Ignore()) // No se actualiza usuario creación
            .ForMember(dest => dest.Geoceqcre, opt => opt.Ignore()) // No se actualiza equipo creación
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => ConvertObjectToJson(src.Geoccoor))) // Conversión de coordenadas
            .ForMember(dest => dest.Geocfedi, opt => opt.MapFrom(src => DateTime.Now)) // Fecha edición automática
            .ForMember(dest => dest.Geogyus, opt => opt.Ignore()); // No se tocan las relaciones

        CreateMap<Geogeoc, GeocercaListDto>();
        CreateMap<Geogyu, VendedorListDto>();
        
        // Mapeo para crear geocerca con vendedores
        CreateMap<GeocercaConVendedoresCreateDto, Geogeoc>()
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => ConvertObjectToJson(src.Geoccoor)))
            .ForMember(dest => dest.Geocfcre, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geocfedi, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geocusedi, opt => opt.MapFrom(src => src.Geocuscre))
            .ForMember(dest => dest.Geoceqedi, opt => opt.MapFrom(src => src.Geoceqcre))
            .ForMember(dest => dest.Geogyus, opt => opt.Ignore()); // Los vendedores se manejan por separado

        // Mapeo de VendedorCreateDto a Geogyu
        CreateMap<VendedorCreateDto, Geogyu>()
            .ForMember(dest => dest.Geugid, opt => opt.Ignore()) // Auto-generado
            .ForMember(dest => dest.Geugidg, opt => opt.Ignore()) // Se asigna en el servicio
            .ForMember(dest => dest.Geugfcre, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geugfedi, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Geugusedi, opt => opt.MapFrom(src => src.Geuguscre))
            .ForMember(dest => dest.Geugeqedi, opt => opt.MapFrom(src => src.Geugeqcre));
        
        // Mapeo de Geogeoc a GeocercaConVendedorDto
        CreateMap<Geogeoc, GeocercaConVendedorDto>()
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => ConvertJsonToObject(src.Geoccoor)))
            .ForMember(dest => dest.Vendedores, opt => opt.MapFrom(src => src.Geogyus));

        // Para obtener detalle de geocerca
        CreateMap<Geogeoc, GeocercaDetailDto>()
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => ConvertJsonToObject(src.Geoccoor)));

        CreateMap<GeocercaDetailDto, GeocercaUpdateDto>()
            .ForMember(dest => dest.Geoccoor, opt => opt.MapFrom(src => src.Geoccoor)) // Ya es object
            .ForMember(dest => dest.Geocusedi, opt => opt.Ignore()) // Se debe establecer desde el contexto
            .ForMember(dest => dest.Geoceqedi, opt => opt.Ignore()); // Se debe establecer desde el contexto
    }

    /// <summary>
    /// Convierte un objeto a JSON string para almacenar en BD
    /// </summary>
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

    /// <summary>
    /// Convierte un JSON string a objeto para usar en DTOs
    /// </summary>
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