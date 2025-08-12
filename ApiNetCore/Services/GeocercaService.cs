using System.Text.Json;
using ApiNetCore.ContextMysql;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Paginacion;
using ApiNetCore.Dtos.Vendedor.Listas;
using ApiNetCore.Entities;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Services;

public class GeocercaService(MyDbContextMysql dbContextMysql, IMapper mapper) : IGeocercaService
{
    public async Task<CreateGeocercaDto> CreateGeocerca(CreateGeocercaDto createGeocercaDto)
    {
        if (await dbContextMysql.TblGeocercas.AnyAsync(x => x.Codigo == createGeocercaDto.Codigo))
            throw new ConflictException($"El código {createGeocercaDto.Codigo} ya existe en la base de datos");
        
        if (await dbContextMysql.TblGeocercas.AnyAsync(x => x.Nombre == createGeocercaDto.Nombre))
            throw new ConflictException($"El nombre {createGeocercaDto.Nombre} ya existe en la base de datos");

        // Validar formato JSON del polígono
        ValidateJsonFormat(createGeocercaDto.PoligonoCoordenadas);

        var newGeocerca = mapper.Map<CreateGeocercaDto, Geocerca>(createGeocercaDto);
        newGeocerca.CreadoEn = DateTime.Now;
        
        await dbContextMysql.TblGeocercas.AddAsync(newGeocerca);
        await dbContextMysql.SaveChangesAsync();
        
        return createGeocercaDto;
    }
    public async Task<CreateGeocercaVendedorDto> CreateGeocercaForVendedor(int idVendedor, CreateGeocercaVendedorDto createGeocercaVendedorDto)
    {
        // Validar que el vendedor existe
        var vendedor = await dbContextMysql.TblGeocercaVendedors.FindAsync(idVendedor);
        if (vendedor == null)
            throw new NotFoundException($"No se encontró el vendedor con ID {idVendedor}");

        // Validaciones de duplicados
        if (await dbContextMysql.TblGeocercas.AnyAsync(x => x.Codigo == createGeocercaVendedorDto.Codigo))
            throw new ConflictException($"El código {createGeocercaVendedorDto.Codigo} ya existe en la base de datos");
        
        if (await dbContextMysql.TblGeocercas.AnyAsync(x => x.Nombre == createGeocercaVendedorDto.Nombre))
            throw new ConflictException($"El nombre {createGeocercaVendedorDto.Nombre} ya existe en la base de datos");

        // Validar formato JSON del polígono
        ValidateJsonFormat(createGeocercaVendedorDto.PoligonoCoordenadas);

        // Crear la geocerca
        var newGeocerca = mapper.Map<CreateGeocercaVendedorDto, Geocerca>(createGeocercaVendedorDto);
        newGeocerca.CreadoEn = DateTime.Now;
        
        await dbContextMysql.TblGeocercas.AddAsync(newGeocerca);
        await dbContextMysql.SaveChangesAsync();

        // Crear la relación en la tabla intermedia
        var geoVen = new GeoVen
        {
            IdVendedor = idVendedor,
            IdGeocerca = newGeocerca.IdGeo,
            CreadoEn = DateTime.Now
        };

        await dbContextMysql.TblGeoVens.AddAsync(geoVen);
        await dbContextMysql.SaveChangesAsync();
        
        return createGeocercaVendedorDto;
    }

    public async Task<UpdateGeocercaDto> UpdateGeocerca(int idGeocerca, UpdateGeocercaDto updateGeocercaDto)
    {
        var geocerca = await dbContextMysql.TblGeocercas.FindAsync(idGeocerca);
        if (geocerca == null)
            throw new NotFoundException($"No se encontró la geocerca con ID {idGeocerca}");

        // Validar duplicados excluyendo el registro actual
        if (await dbContextMysql.TblGeocercas.AnyAsync(x => x.Codigo == updateGeocercaDto.Codigo && x.IdGeo != idGeocerca))
            throw new ConflictException($"El código {updateGeocercaDto.Codigo} ya existe en otra geocerca");
        
        if (await dbContextMysql.TblGeocercas.AnyAsync(x => x.Nombre == updateGeocercaDto.Nombre && x.IdGeo != idGeocerca))
            throw new ConflictException($"El nombre {updateGeocercaDto.Nombre} ya existe en otra geocerca");

        // Validar formato JSON del polígono
        ValidateJsonFormat(updateGeocercaDto.PoligonoCoordenadas);

        // Mapear cambios
        mapper.Map(updateGeocercaDto, geocerca);
        geocerca.ActualizadoEn = DateTime.Now;
        
        await dbContextMysql.SaveChangesAsync();
        return updateGeocercaDto;
    }

    public async Task<bool> DeleteGeocerca(int idGeocerca)
    {
        var geocerca = await dbContextMysql.TblGeocercas.FindAsync(idGeocerca);
        if (geocerca == null)
            throw new NotFoundException($"No se encontró la geocerca con ID {idGeocerca}");

        // Verificar si tiene vendedores asociados
        var tieneVendedores = await dbContextMysql.TblGeoVens.AnyAsync(gv => gv.IdGeocerca == idGeocerca);
        if (tieneVendedores)
            throw new ConflictException("No se puede eliminar la geocerca porque tiene vendedores asociados");

        dbContextMysql.TblGeocercas.Remove(geocerca);
        await dbContextMysql.SaveChangesAsync();
        return true;
    }

    public async Task<LGeocercaDto> GetGeocercasAsync(int pagina = 1, int tamanioPagina = 10)
    {
        try
        {
            if (pagina < 1) pagina = 1;
            if (tamanioPagina is < 1 or > 100) tamanioPagina = 10;

            var totalGeocercas = await dbContextMysql.TblGeocercas.CountAsync();
            
            if (totalGeocercas == 0) 
                throw new NotFoundException("No se encontraron geocercas en la base de datos");

            // Incluir vendedores a través de la tabla intermedia
            var geocercas = await dbContextMysql.TblGeocercas
                .Include(g => g.TblGeoVens)
                    .ThenInclude(gv => gv.IdVendedorNavigation)
                .OrderBy(g => g.Codigo)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();

            var geocercasDto = mapper.Map<List<Geocerca>, List<GeocercaConVendedorDto>>(geocercas);
            
            var totalPaginas = (int)Math.Ceiling((double)totalGeocercas / tamanioPagina);
            
            var resultado = new LGeocercaDto
            {
                Geocercas = geocercasDto,
                Paginacion = new PaginacionDto
                {
                    PaginaActual = pagina,
                    TamanioPagina = tamanioPagina,
                    TotalRegistros = totalGeocercas,
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

    public async Task<GeocercaConVendedorDto> GetGeocercaByIdAsync(int idGeocerca)
    {
        var geocerca = await dbContextMysql.TblGeocercas
            .Include(g => g.TblGeoVens)
                .ThenInclude(gv => gv.IdVendedorNavigation)
            .FirstOrDefaultAsync(g => g.IdGeo == idGeocerca);

        if (geocerca == null)
            throw new NotFoundException($"No se encontró la geocerca con ID {idGeocerca}");

        return mapper.Map<Geocerca, GeocercaConVendedorDto>(geocerca);
    }

    private static void ValidateJsonFormat(string? poligonoCoordenadas)
    {
        if (string.IsNullOrEmpty(poligonoCoordenadas))
            throw new BadRequestException("El polígono de coordenadas es requerido");

        try
        {
            JsonDocument.Parse(poligonoCoordenadas);
        }
        catch (JsonException ex)
        {
            throw new BadRequestException($"El polígono de coordenadas debe tener un formato JSON válido: {ex.Message}");
        }
    }
}