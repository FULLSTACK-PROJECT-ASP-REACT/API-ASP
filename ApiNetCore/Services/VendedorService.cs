using System.Text.Json;
using ApiNetCore.ContextMysql;
using ApiNetCore.Entities;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ApiNetCore.Dtos.Paginacion;
using ApiNetCore.Dtos.Vendedor;
using ApiNetCore.Dtos.Vendedor.Listas;

namespace ApiNetCore.Services;

public class VendedorService(MyDbContextMysql dbContextMysql, IMapper mapper) : IVendedorService
{
    public async Task<CreateVendedorDto> CreateVendedor(CreateVendedorDto createVendedorDto)
    {
        if (await dbContextMysql.TblGeocercaVendedors.AnyAsync(x => x.CodeVendedor == createVendedorDto.CodeVendedor))
            throw new ConflictException($"El codigo {createVendedorDto.CodeVendedor} ya existe en la base de datos");
    
        if (await dbContextMysql.TblGeocercaVendedors.AnyAsync(x => x.NombreVendedor == createVendedorDto.NombreVendedor))
            throw new ConflictException($"El nombre {createVendedorDto.NombreVendedor} ya existe en la base de datos");
    
        if (string.IsNullOrEmpty(createVendedorDto.CoordenadasVendedor) && 
            createVendedorDto is { LatitudVendedor: not null, LongitudVendedor: not null })
        {
            createVendedorDto.CoordenadasVendedor = BuildCoordinatesJson(
                createVendedorDto.LatitudVendedor, 
                createVendedorDto.LongitudVendedor);
        }
    
        if (!string.IsNullOrEmpty(createVendedorDto.CoordenadasVendedor))
        {
            try
            {
                JsonDocument.Parse(createVendedorDto.CoordenadasVendedor);
            }
            catch (JsonException)
            {
                throw new BadRequestException("Las coordenadas deben tener un formato JSON válido");
            }
        }
    
        var newVendedor = mapper.Map<CreateVendedorDto, GeocercaVendedor>(createVendedorDto);
        await dbContextMysql.TblGeocercaVendedors.AddAsync(newVendedor);
        await dbContextMysql.SaveChangesAsync();
        return createVendedorDto;
    }

    public async Task<LVendedorDto> GetVendedoresAsync(int pagina = 1, int tamanioPagina = 10)
    {
        try
        {
            if (pagina < 1) pagina = 1;
            if (tamanioPagina is < 1 or > 100) tamanioPagina = 10;

            var totalVendedores = await dbContextMysql.TblGeocercaVendedors.CountAsync();
        
            if (totalVendedores == 0) 
                throw new NotFoundException("No se encontraron vendedores en la base de datos");

            var vendedores = await dbContextMysql.TblGeocercaVendedors
                .OrderBy(v => v.CodeVendedor) // Ordenar por código (o el campo que prefieras)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();

            var vendedoresDto = mapper.Map<List<GeocercaVendedor>, List<VendedorDto>>(vendedores);
        
            var totalPaginas = (int)Math.Ceiling((double)totalVendedores / tamanioPagina);
        
            var resultado = new LVendedorDto
            {
                Vendedores = vendedoresDto,
                Paginacion = new PaginacionDto
                {
                    PaginaActual = pagina,
                    TamanioPagina = tamanioPagina,
                    TotalRegistros = totalVendedores,
                    TotalPaginas = totalPaginas
                },
                FechaConsulta = DateTime.Now
            };
        
            return resultado;
        }
        catch (Exception exception)
        {
            throw new InternalServerException(exception.Message, exception);
        }
    }
    private static string BuildCoordinatesJson(decimal? latitud, decimal? longitud)
    {
        if (!latitud.HasValue || !longitud.HasValue)
            throw new BadRequestException("La latitud y longitud son requeridas");
    
        var lat = latitud.Value;
        var lng = longitud.Value;
    
        if (lat is < -90 or > 90)
            throw new BadRequestException("La latitud debe estar entre -90 y 90 grados");
    
        if (lng is < -180 or > 180)
            throw new BadRequestException("La longitud debe estar entre -180 y 180 grados");
    
        var coordinates = new { lat, lng };
        return JsonSerializer.Serialize(coordinates);
    }
}