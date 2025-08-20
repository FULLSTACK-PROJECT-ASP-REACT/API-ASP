using ApiNetCore.ContextMysql;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Geocerca.GeoUsu;
using ApiNetCore.Dtos.Paginacion;
using ApiNetCore.Dtos.Vendedor;
using ApiNetCore.Exceptions;
using ApiNetCore.Models;
using ApiNetCore.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Services;

public class GeocercaService : IGeocercaService
{
    private MyDbContextMysql _dbContextMysql;
    private readonly IMapper _mapper;

    public GeocercaService(MyDbContextMysql dbContextMysql, IMapper mapper)
    {
        _dbContextMysql = dbContextMysql;
        _mapper = mapper;
    }


    public async Task<PaginatedResultDto<GeocercaListDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10,
        string? searchTerm = null, string? estado = null, bool? activo = null)
    {
        try
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("El número de página y el tamaño de página deben ser mayores a 0");

            var query = _dbContextMysql.Geogeocs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(g =>
                    g.Geocnom.Contains(searchTerm) || g.Geoccod.Contains(searchTerm) ||
                    g.Geocsec.Contains(searchTerm) || g.Geocciud.Contains(searchTerm));
            if (!string.IsNullOrEmpty(estado)) query = query.Where(g => g.Geocest == estado);

            if (activo.HasValue) query = query.Where(g => g.Geocact == activo.Value);

            var totalItems = await query.CountAsync();

            // Aplicar paginación
            var geocercas = await query
                .OrderBy(g => g.Geocnom)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var geocercasDto = _mapper.Map<List<GeocercaListDto>>(geocercas);

            var paginacion = new PaginacionDto
            {
                PaginaActual = pageNumber,
                TamanioPagina = pageSize,
                TotalRegistros = totalItems,
                TotalPaginas = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return new PaginatedResultDto<GeocercaListDto>
            {
                Data = geocercasDto,
                Paginacion = paginacion
            };
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException($"Error al obtener las geocercas: {ex.Message}");
        }
    }

    public async Task<PaginatedResultDto<GeocercaConVendedorDto>> GetAllGeocercaConVendedorAsync(int pageNumber = 1,
        int pageSize = 10, string? searchTerm = null,
        string? estado = null, bool? activo = null, bool soloConVendedores = false)
    {
        try
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("El número de página y el tamaño de página deben ser mayores a 0");

            var query = _dbContextMysql.Geogeocs.Include(g => g.Geogyus).AsQueryable();
            // Aplicar filtros
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(g =>
                    g.Geocnom.Contains(searchTerm) ||
                    g.Geoccod.Contains(searchTerm) ||
                    g.Geocsec.Contains(searchTerm) ||
                    g.Geocciud.Contains(searchTerm));

            if (!string.IsNullOrEmpty(estado)) query = query.Where(g => g.Geocest == estado);

            if (activo.HasValue) query = query.Where(g => g.Geocact == activo.Value);

            if (soloConVendedores) query = query.Where(g => g.Geogyus.Count != 0);

            var totalItems = await query.CountAsync();

            // Aplicar paginación
            var geocercas = await query
                .OrderBy(g => g.Geocnom)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var geocercasDto = _mapper.Map<List<GeocercaConVendedorDto>>(geocercas);

            var paginacion = new PaginacionDto
            {
                PaginaActual = pageNumber,
                TamanioPagina = pageSize,
                TotalRegistros = totalItems,
                TotalPaginas = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return new PaginatedResultDto<GeocercaConVendedorDto>
            {
                Data = geocercasDto,
                Paginacion = paginacion
            };
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException($"Error al obtener geocercas con vendedores: {ex.Message}");
        }
    }

    public async Task<GeocercaDetailDto> GetByCodigoAsync(string codigo)
    {
        try
        {
            if (string.IsNullOrEmpty(codigo))
                throw new BadRequestException("El código de geocerca es requerido");

            var geocerca = await _dbContextMysql.Geogeocs
                .FirstOrDefaultAsync(g => g.Geoccod == codigo);

            return geocerca == null
                ? throw new NotFoundException($"No se encontró la geocerca con código: {codigo}")
                : _mapper.Map<GeocercaDetailDto>(geocerca);
        }
        catch (Exception ex) when (!(ex is BadRequestException || ex is NotFoundException))
        {
            throw new InternalServerException($"Error al obtener la geocerca: {ex.Message}");
        }
    }
    
    public async Task<bool> ExistsAsync(string codigo)
    {
        try
        {
            if (string.IsNullOrEmpty(codigo))
                return false;

            return await _dbContextMysql.Geogeocs
                .AnyAsync(g => g.Geoccod == codigo);
        }
        catch (Exception ex)
        {
            throw new InternalServerException($"Error al verificar existencia de geocerca: {ex.Message}");
        }
    }


    public async Task<GeocercaConVendedoresCreateResponseDto> CreateGeocercaConVendedoresAsync(
        GeocercaConVendedoresCreateDto createDto)
    {
        await using var transaction = await _dbContextMysql.Database.BeginTransactionAsync();

        try
        {
            if (createDto == null)
                throw new BadRequestException("Los datos de la geocerca son requeridos");


            var existeGeocerca = await ExistsAsync(createDto.Geoccod);
            if (existeGeocerca)
                throw new ConflictException($"Ya existe una geocerca con el código: {createDto.Geoccod}");

            if (createDto.ValidarVendedoresDuplicados && createDto.Vendedores.Count != 0)
            {
                var vendedoresDuplicados = createDto.Vendedores
                    .GroupBy(v => v.Geugidv)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (vendedoresDuplicados.Count != 0)
                    throw new BadRequestException(
                        $"Los siguientes vendedores tienen duplicados: {string.Join(", ", vendedoresDuplicados)}");
            }

            if (createDto.Vendedores.Count == 0)
                throw new BadRequestException("Se debe proporcionar al menos un vendedor");

            var geocerca = _mapper.Map<Geogeoc>(createDto);
            _dbContextMysql.Add(geocerca);
            await _dbContextMysql.SaveChangesAsync();

            var response = new GeocercaConVendedoresCreateResponseDto
            {
                Geoccod = geocerca.Geoccod,
                Geocnom = geocerca.Geocnom,
                FechaCreacion = geocerca.Geocfcre,
                VendedoresCreados = [],
                ErroresVendedores = []
            };

            if (createDto.Vendedores.Count != 0)
            {
                var resultadoVendedores = await CrearVendedoresInternos(geocerca.Geoccod, createDto.Vendedores);

                response.TotalVendedoresCreados = resultadoVendedores.VendedoresCreados.Count;
                response.VendedoresCreados = resultadoVendedores.VendedoresCreados;

                if (resultadoVendedores.ErroresVendedores.Count != 0)
                    response.ErroresVendedores = resultadoVendedores.ErroresVendedores;
            }

            var geocercaDetalle = await GetByCodigoAsync(geocerca.Geoccod);
            response.DetalleGeocerca = geocercaDetalle;

            await transaction.CommitAsync();

            response.Mensaje =
                $"Geocerca '{geocerca.Geocnom}' creada exitosamente con {response.TotalVendedoresCreados} vendedores asignados";

            return response;
        }
        catch (Exception ex) when (ex is not (BadRequestException or ConflictException or ValidationException))
        {
            await transaction.RollbackAsync();
            throw new InternalServerException($"Error al crear geocerca con vendedores: {ex.Message}");
        }
    }


    private async Task<(List<string> VendedoresCreados, List<string> ErroresVendedores)> CrearVendedoresInternos(string codigoGeocerca, List<VendedorCreateDto> vendedores)
    {
        var vendedoresCreados = new List<string>();
        var erroresVendedores = new List<string>();

        foreach (var vendedorDto in vendedores)
            try
            {
                var existeVendedor = await _dbContextMysql.Geogyus
                    .AnyAsync(v => v.Geugidv == vendedorDto.Geugidv && v.Geugidg == codigoGeocerca);

                if (existeVendedor)
                {
                    erroresVendedores.Add($"El vendedor {vendedorDto.Geugidv} ya está asignado a esta geocerca");
                    continue;
                }

                var vendedor = _mapper.Map<Geogyu>(vendedorDto);
                vendedor.Geugidg = codigoGeocerca;

                _dbContextMysql.Add(vendedor);
                await _dbContextMysql.SaveChangesAsync();

                vendedoresCreados.Add(vendedorDto.Geugidv);
            }
            catch (Exception ex)
            {
                erroresVendedores.Add($"Error al crear vendedor {vendedorDto.Geugidv}: {ex.Message}");
            }

        return (vendedoresCreados, erroresVendedores);
    }
}