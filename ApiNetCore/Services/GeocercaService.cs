using ApiNetCore.ContextMysql;
using ApiNetCore.Dtos;
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
    private readonly MyDbContextMysql _dbContextMysql;
    private readonly IMapper _mapper;
    private readonly IVendedorExternoService _vendedorExternoService;


    public GeocercaService(MyDbContextMysql dbContextMysql, IMapper mapper,
        IVendedorExternoService vendedorExternoService)
    {
        _dbContextMysql = dbContextMysql;
        _mapper = mapper;
        _vendedorExternoService = vendedorExternoService;
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
        string? estado = null, bool? activo = null, bool soloConVendedores = false, string nameEnterprise = "MEVECSA")
    {
            if (string.IsNullOrEmpty((nameEnterprise)))
                throw new BadRequestException("El nombre de la empresa es requerido");
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("El número de página y el tamaño de página deben ser mayores a 0");
            try
            {

                var query = _dbContextMysql.Geogeocs.Include(g => g.Geogyus).AsQueryable()
                    .Where(g => g.Geoceqcre.Contains(nameEnterprise));

            
                // Aplicar filtros
                if (!string.IsNullOrEmpty(searchTerm))
                    query = query.Where(g =>
                        g.Geocnom.Contains(searchTerm) ||
                        g.Geoccod.Contains(searchTerm) ||
                        g.Geocsec.Contains(searchTerm) ||
                        g.Geocciud.Contains(searchTerm));

                if (!string.IsNullOrEmpty(estado)) query = query.Where(g => g.Geocest == estado);

                if (activo.HasValue) query = query.Where(g => g.Geocact == activo.Value);

                query = soloConVendedores ? query.Where(g => g.Geogyus.Count > 0) : // CON vendedores
                    query.Where(g => g.Geogyus.Count == 0); // SIN vendedores

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
                throw new BadRequestException("El código de geocerca es requerido");

            var geocerca = await _dbContextMysql.Geogeocs
                .FirstOrDefaultAsync(g => g.Geoccod == codigo);
            
            return geocerca != null; 
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InternalServerException($"Error al verificar existencia de geocerca: {ex.Message}");
        }
    }

    public async Task<GeocercaUpdateResponseDto> CreateAsync(GeocercaCreateDto createDto)
    {
        if(createDto == null || string.IsNullOrWhiteSpace(createDto.Geoccod))
            throw new BadRequestException("Los datos de la geocerca son requeridos");
            
        if(createDto.Geoccod == "string")
            throw new BadRequestException("El código de geocerca debe ser un valor válido");
            
        var existeGeocerca = await ExistsAsync(createDto.Geoccod);
        if (existeGeocerca)
            throw new ConflictException($"Ya existe una geocerca con el código: {createDto.Geoccod}");
        try
        {
            var geocerca = _mapper.Map<Geogeoc>(createDto);
            _dbContextMysql.Add(geocerca);
            await _dbContextMysql.SaveChangesAsync();

            var geocercaDetalle = _mapper.Map<GeocercaDetailDto>(geocerca);

            var response = new GeocercaUpdateResponseDto
            {
                Geoccod = geocercaDetalle.Geoccod,
                Geocnom = geocercaDetalle.Geocnom,
                FechaEdicion = geocercaDetalle.Geocfedi,
                UsuarioEditor = geocercaDetalle.Geocusedi,
                Mensaje = $"Geocerca '{geocercaDetalle.Geocnom}' creada exitosamente",
                DetalleGeocerca = geocercaDetalle
            };

            return response;
            
        }
        catch (Exception ex) when (ex is not (BadRequestException or NotFoundException or ConflictException))        {
            throw new InternalServerException($"Error al crear geocerca: {ex.Message}");
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

    public async Task<VendorGeofenceAssignmentDto> CreateGeocercaConVendedorAsync(VendorGeofenceAssignmentDto createDto, string codigoGeocerca)
    {
        if (string.IsNullOrEmpty(codigoGeocerca))
            throw new BadRequestException("El código de la geocerca es requerido");
    
        if (createDto == null)
            throw new BadRequestException("Los datos de la geocerca son requeridos");
    
        var existeGeocerca = await ExistsAsync(codigoGeocerca);
        if (!existeGeocerca)
            throw new NotFoundException($"No se encontró una geocerca con el código: {codigoGeocerca}");

        var existeAsignacion = await ExistsVendorGeofenceAssignmentAsync(codigoGeocerca, createDto.Geugidv);
        if (existeAsignacion)
            throw new ConflictException($"Ya existe una asignación entre la geocerca {codigoGeocerca} y el vendedor {createDto.Geugidv}");

        try
        {
            var assignment = _mapper.Map<Geogyu>(createDto);
            assignment.Geugidg = codigoGeocerca;
        
            _dbContextMysql.Add(assignment);
            await _dbContextMysql.SaveChangesAsync();

            return _mapper.Map<VendorGeofenceAssignmentDto>(assignment);
        }
        catch (Exception ex) when (ex is not (BadRequestException or NotFoundException or ConflictException))
        {
            throw new InternalServerException($"Error al crear asignación geocerca-vendedor: {ex.Message}");
        }
    }

    public async Task<PaginatedResultDto<GeocercaListDto>> GetListGeofenceByEnterpriseAsync(int pageNumber = 1, int pageSize = 10, bool? activo = null,
        string nameEnterprise = "MEVECSA")
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new BadRequestException("El número de página y el tamaño de página deben ser mayores a 0");
        if (string.IsNullOrEmpty(nameEnterprise))
            throw new BadRequestException("El nombre de la empresa es requerido.");
        try
        {
            var query = _dbContextMysql.Set<Geogeoc>()
                .Where(g => g.Geoceqcre.Contains(nameEnterprise));
            
            if (activo.HasValue)
                query = query.Where(g => g.Geocact == activo.Value);
            
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            var itemsDto = _mapper.Map<List<GeocercaListDto>>(items);
            
            var paginacion = new PaginacionDto
            {
                PaginaActual = pageNumber,
                TamanioPagina = pageSize
            };
            
            return new PaginatedResultDto<GeocercaListDto>
            {
                Data = itemsDto,
                Paginacion = paginacion
            };
        

        }
        catch (Exception ex) when (ex is not (BadRequestException or NotFoundException or ConflictException))
        {
            throw new InternalServerException($"Error al obtener listado de geocercas: {ex.Message}");
        }
    }

    private async Task<bool> ExistsVendorGeofenceAssignmentAsync(string geocod, string vendorId)
    {
        return await _dbContextMysql.Set<Geogyu>()
            .AnyAsync(g => g.Geugidg == geocod && g.Geugidv == vendorId);
    }

    public async Task<GeocercaUpdateResponseDto> UpdateAsync(string codigo, GeocercaUpdateDto updateDto)
    {
        try
        {
            if (string.IsNullOrEmpty(codigo))
                throw new BadRequestException("El código de la geocerca es requerido");

            if (updateDto == null)
                throw new BadRequestException("Los datos de la geocerca son requeridos");

            var geocercaEntity = await _dbContextMysql.Geogeocs
                .FirstOrDefaultAsync(g => g.Geoccod == codigo);

            if (geocercaEntity == null)
                throw new NotFoundException($"No se encontró una geocerca con el código: {codigo}");

            _mapper.Map(updateDto, geocercaEntity);

            _dbContextMysql.Update(geocercaEntity);
            await _dbContextMysql.SaveChangesAsync();

            var geocercaDetalle = _mapper.Map<GeocercaDetailDto>(geocercaEntity);

            var response = new GeocercaUpdateResponseDto
            {
                Geoccod = geocercaDetalle.Geoccod,
                Geocnom = geocercaDetalle.Geocnom,
                FechaEdicion = geocercaDetalle.Geocfedi,
                UsuarioEditor = geocercaDetalle.Geocusedi,
                Mensaje = $"Geocerca '{geocercaDetalle.Geocnom}' actualizada exitosamente",
                DetalleGeocerca = geocercaDetalle
            };

            return response;
        }
        catch (Exception ex) when (ex is not (BadRequestException or NotFoundException))
        {
            throw new InternalServerException($"Error al actualizar geocerca: {ex.Message}");
        }
    }

    public async Task<GeocercaUpdateResponseDto> DeleteAsync(string codigo)
    {
        try
        {
            if (string.IsNullOrEmpty(codigo))
                throw new BadRequestException("El código de la geocerca es requerido");

            var geocercaEntity = await _dbContextMysql.Geogeocs
                .FirstOrDefaultAsync(g => g.Geoccod == codigo);

            var geocercaVendedores = await _dbContextMysql.Geogyus
                .Where(v => v.Geugidg == codigo)
                .ToListAsync();

            foreach (var geocercaVendedor in geocercaVendedores)
                _dbContextMysql.Remove(geocercaVendedor);

            if (geocercaEntity == null)
                throw new NotFoundException($"No se encontró una geocerca con el código: {codigo}");

            _dbContextMysql.Remove(geocercaEntity);
            await _dbContextMysql.SaveChangesAsync();
        }
        catch (Exception ex) when (ex is not (BadRequestException or NotFoundException))
        {
            throw new InternalServerException($"Error al eliminar geocerca: {ex.Message}");
        }

        return new GeocercaUpdateResponseDto
        {
            Geoccod = codigo,
            Mensaje = $"Geocerca '{codigo}' eliminada exitosamente"
        };
    }
    
    

    public async Task<GeocercaUpdateResponseDto> DesactivarAsync(string codigo)
    {
        try
        {
            if (string.IsNullOrEmpty(codigo))
                throw new BadRequestException("El código de la geocerca es requerido");
            
            
            var geocercaEntity = await _dbContextMysql.Geogeocs
                .FirstOrDefaultAsync(g => g.Geoccod == codigo);
            
            
            if (geocercaEntity == null)
                throw new NotFoundException($"No se encontró una geocerca con el código: {codigo}");
            
            if (geocercaEntity.Geocest == "I")
                throw new BadRequestException($"La geocerca '{geocercaEntity.Geocnom}' ya se encuentra desactivada");
            
            
            geocercaEntity.Geocact = false;
            geocercaEntity.Geocfedi = DateTime.Now;
            geocercaEntity.Geocest = "I";
            _dbContextMysql.Update(geocercaEntity);
            await _dbContextMysql.SaveChangesAsync();
            return new GeocercaUpdateResponseDto
            {
                Geoccod = geocercaEntity.Geoccod,
                Geocnom = geocercaEntity.Geocnom,
                FechaEdicion = geocercaEntity.Geocfedi,
                UsuarioEditor = geocercaEntity.Geocusedi,
                Mensaje = $"Geocerca '{geocercaEntity.Geocnom}' desactivada exitosamente"
            };
        }
        catch (Exception ex) when (ex is not (BadRequestException or NotFoundException))
        {
            throw new InternalServerException($"Error al desactivar geocerca: {ex.Message}");
        }
    }

    public async Task<GeocercaUpdateResponseDto> ActivarAsync(string codigo)
    {
        try
        {
            if (string.IsNullOrEmpty(codigo))
                throw new BadRequestException("El código de la geocerca es requerido");
            
            var geocercaEntity = await _dbContextMysql.Geogeocs
                .FirstOrDefaultAsync(g => g.Geoccod == codigo);
            
            if (geocercaEntity == null)
                throw new NotFoundException($"No se encontró una geocerca con el código: {codigo}");
            
            
            if (geocercaEntity.Geocest == "A")
                throw new BadRequestException($"La geocerca '{geocercaEntity.Geocnom}' ya se encuentra activa");
            
            geocercaEntity.Geocact = true;
            geocercaEntity.Geocfedi = DateTime.Now;
            geocercaEntity.Geocest = "A";
            _dbContextMysql.Update(geocercaEntity);
            await _dbContextMysql.SaveChangesAsync();
            return new GeocercaUpdateResponseDto
            {
                Geoccod = geocercaEntity.Geoccod,
                Geocnom = geocercaEntity.Geocnom,
                FechaEdicion = geocercaEntity.Geocfedi,
                UsuarioEditor = geocercaEntity.Geocusedi,
                Mensaje = $"Geocerca '{geocercaEntity.Geocnom}' desactivada exitosamente"
            };
        }
        catch (Exception ex) when (ex is not (BadRequestException or NotFoundException))
        {
            throw new InternalServerException($"Error al desactivar geocerca: {ex.Message}");
        }    
    }

    public async Task<GeocercaUpdateResponseDto> DesvincularVendedorAsync(string codigo)
    {
        if (string.IsNullOrEmpty(codigo))
            throw new BadRequestException("El código de la geocerca es requerido");

        if (!await ExistsAsync(codigo))
            throw new NotFoundException($"No se encontró una geocerca con el código: {codigo}");
        try
        {
            var geocercaEntity = await _dbContextMysql.Geogyus
                .FirstOrDefaultAsync(g => g.Geugidg == codigo);
            
            if (geocercaEntity == null)
                throw new NotFoundException($"No se encontró una geocerca con el código: {codigo}");
            
            _dbContextMysql.Remove(geocercaEntity);
            await _dbContextMysql.SaveChangesAsync();
            
            return new GeocercaUpdateResponseDto
            {
                Geoccod = geocercaEntity.Geugidv,
                Geocnom = geocercaEntity.Geugidg,
                FechaEdicion = geocercaEntity.Geugfedi,
                UsuarioEditor = geocercaEntity.Geugusedi,
                Mensaje = $"Geocerca '{geocercaEntity.Geugidv}' desactivada exitosamente"
            };
            
        }
        catch (Exception ex) when (ex is not (BadRequestException or NotFoundException))
        {
            throw new InternalServerException($"Error al desvincular vendedor: {ex.Message}");
        }
    }

    public async Task<GeocercaUpdateResponseDto> ConsultarAsync(string codigo)
    {
        if (string.IsNullOrEmpty(codigo))
            throw new BadRequestException("El código de la geocerca es requerido");

        try
        {
            var geocercaEntity = await _dbContextMysql.Geogyus
                .FirstOrDefaultAsync(g => g.Geugidg == codigo);
        
            if (geocercaEntity == null)
                throw new NotFoundException($"La geocerca {codigo} que mencionas no tiene relacion con ningún vendedor: ");
        
            return new GeocercaUpdateResponseDto
            {
                Geoccod = geocercaEntity.Geugidg, 
                Geocnom = geocercaEntity.Geugidv, 
                FechaEdicion = geocercaEntity.Geugfedi,
                UsuarioEditor = geocercaEntity.Geugusedi,
                Mensaje = $"La Geocerca '{geocercaEntity.Geugidg}' tiene relacion con el vendedor '{geocercaEntity.Geugidv}'"
            };
        }catch (Exception ex) when (ex is not (BadRequestException or NotFoundException))
        {
            throw new InternalServerException($"Error al consultar geocerca: {ex.Message}");
        }
    }


    public async Task<PaginatedResultDto<VendedorConGeocercasDto>> GetVendedoresConGeocercasAsync(string token, int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? activo = null, string? estado = null)
    {
        if (string.IsNullOrEmpty(token))
            throw new BadRequestException("Token de API externo es requerido");

        if (pageNumber <= 0 || pageSize <= 0)
            throw new BadRequestException("El número de página y el tamaño de página deben ser mayores a 0");
        try
        {

            var vendedoresExternos = await _vendedorExternoService.GetVendedoresAsync(token);

            if (vendedoresExternos.Count == 0)
                return new PaginatedResultDto<VendedorConGeocercasDto>
                {
                    Data = [],
                    Paginacion = new PaginacionDto
                    {
                        PaginaActual = pageNumber,
                        TamanioPagina = pageSize,
                        TotalRegistros = 0,
                        TotalPaginas = 0
                    }
                };

            var codigosVendedores = vendedoresExternos.Select(v => v.Usucod).ToList();

            var queryGeogyus = _dbContextMysql.Geogyus
                .Include(g => g.GeugidgNavigation)
                .Where(g => codigosVendedores.Contains(g.Geugidv))
                .AsQueryable();

            if (activo.HasValue)
                queryGeogyus = queryGeogyus.Where(g => g.GeugidgNavigation.Geocact == activo.Value);

            if (!string.IsNullOrEmpty(estado))
                queryGeogyus = queryGeogyus.Where(g => g.GeugidgNavigation.Geocest == estado);

            var vendedoresConGeocercas = new List<VendedorConGeocercasDto>();

            foreach (var vendedorExterno in vendedoresExternos)
            {
                var geocercasVendedor = await queryGeogyus
                    .Where(g => g.Geugidv == vendedorExterno.Usucod)
                    .Select(g => new GeocercaVendedorDto
                    {
                        Geoccod = g.GeugidgNavigation.Geoccod,
                        Geocnom = g.GeugidgNavigation.Geocnom,
                        Geocsec = g.GeugidgNavigation.Geocsec,
                        Geocciud = g.GeugidgNavigation.Geocciud,
                        Geocdirre = g.GeugidgNavigation.Geocdirre,
                        Geocprov = g.GeugidgNavigation.Geocprov,
                        Geocest = g.GeugidgNavigation.Geocest,
                        Geocact = g.GeugidgNavigation.Geocact,
                        Geocpri = g.GeugidgNavigation.Geocpri,
                        Geoclat = g.GeugidgNavigation.Geoclat,
                        Geoclon = g.GeugidgNavigation.Geoclon,
                        Geocarm = g.GeugidgNavigation.Geocarm,
                        Geocperm = g.GeugidgNavigation.Geocperm,
                        Geoccoor = g.GeugidgNavigation.Geoccoor,
                        FechaAsignacion = g.Geugfcre
                    })
                    .ToListAsync();

                var vendedorConGeocercas = new VendedorConGeocercasDto
                {
                    CodigoVendedor = vendedorExterno.Usucod,
                    NombreVendedor = vendedorExterno.Usunombre,
                    EmailVendedor = vendedorExterno.Usuemail,
                    CodigoVendedorSecundario = vendedorExterno.Usucodv,
                    UbicacionActual = vendedorExterno.Ubicacion,
                    Geocercas = geocercasVendedor,
                    TotalGeocercas = geocercasVendedor.Count
                };

                vendedoresConGeocercas.Add(vendedorConGeocercas);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                vendedoresConGeocercas = vendedoresConGeocercas
                    .Where(v =>
                        v.CodigoVendedor.Contains(searchTermLower, StringComparison.CurrentCultureIgnoreCase) ||
                        v.NombreVendedor.Contains(searchTermLower, StringComparison.CurrentCultureIgnoreCase) ||
                        v.EmailVendedor.Contains(searchTermLower, StringComparison.CurrentCultureIgnoreCase) ||
                        v.Geocercas.Any(g =>
                            g.Geocnom.Contains(searchTermLower, StringComparison.CurrentCultureIgnoreCase) ||
                            g.Geocsec.Contains(searchTermLower, StringComparison.CurrentCultureIgnoreCase) ||
                            g.Geocciud.Contains(searchTermLower, StringComparison.CurrentCultureIgnoreCase)))
                    .ToList();
            }

            var totalItems = vendedoresConGeocercas.Count;
            var vendedoresPaginados = vendedoresConGeocercas
                .OrderBy(v => v.NombreVendedor)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paginacion = new PaginacionDto
            {
                PaginaActual = pageNumber,
                TamanioPagina = pageSize,
                TotalRegistros = totalItems,
                TotalPaginas = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return new PaginatedResultDto<VendedorConGeocercasDto>
            {
                Data = vendedoresPaginados,
                Paginacion = paginacion
            };
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException($"Error al obtener vendedores con geocercas: {ex.Message}");
        }
    }


    private async Task<(List<string> VendedoresCreados, List<string> ErroresVendedores)> CrearVendedoresInternos(
        string codigoGeocerca, List<VendedorCreateDto> vendedores)
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