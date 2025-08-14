using ApiNetCore.ContextMysql;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Geocerca.GeoUsu;
using ApiNetCore.Dtos.Paginacion;
using ApiNetCore.Exceptions;
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
    
    
    public async Task<PaginatedResultDto<GeocercaListDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? estado = null, bool? activo = null)
    {
        try
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("El número de página y el tamaño de página deben ser mayores a 0");
            
            var query = _dbContextMysql.Geogeocs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(g => g.Geocnom.Contains(searchTerm) || g.Geoccod.Contains(searchTerm) || g.Geocsec.Contains(searchTerm) || g.Geocciud.Contains(searchTerm));                
            }
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(g => g.Geocest == estado);
            }

            if (activo.HasValue)
            {
                query = query.Where(g => g.Geocact == activo.Value);
            }
            
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

    public async Task<PaginatedResultDto<GeocercaConVendedorDto>> GetAllGeocercaConVendedorAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null,
        string? estado = null, bool? activo = null , bool soloConVendedores = false)
    {
        try
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("El número de página y el tamaño de página deben ser mayores a 0");
            
            var query = _dbContextMysql.Geogeocs.Include(g => g.Geogyus).AsQueryable();
            // Aplicar filtros
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(g => 
                    g.Geocnom.Contains(searchTerm) ||
                    g.Geoccod.Contains(searchTerm) ||
                    g.Geocsec.Contains(searchTerm) ||
                    g.Geocciud.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(g => g.Geocest == estado);
            }

            if (activo.HasValue)
            {
                query = query.Where(g => g.Geocact == activo.Value);
            }

            // Filtrar solo geocercas que tienen vendedores asignados
            if (soloConVendedores)
            {
                query = query.Where(g => g.Geogyus.Any());
            }
            
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
        throw new NotImplementedException();
    }

    public async Task<GeocercaDetailDto> CreateAsync(GeocercaCreateDto createDto)
    {
        throw new NotImplementedException();
    }

    public async Task<GeocercaDetailDto> UpdateAsync(string codigo, GeocercaUpdateDto updateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(string codigo)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsAsync(string codigo)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ToggleActiveAsync(string codigo, bool activo)
    {
        throw new NotImplementedException();
    }
}