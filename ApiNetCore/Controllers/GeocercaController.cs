using System.Diagnostics;
using ApiNetCore.Dtos;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Geocerca.GeoUsu;
using ApiNetCore.Dtos.Paginacion;
using ApiNetCore.Dtos.Usuario;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore.Controllers;
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class GeocercaController(IGeocercaService geocercaService, IVendedorCoordenadasService vendedorCoordenadasService) : ControllerBase
{
    
    [HttpPost("coordenadas-vendedores")]
    public async Task<ActionResult<ApiResponse<List<VendedorCoordenadasDto>>>> GetListCoordenadasVendedorAsync([FromBody]GetListCoordenadasVendedorRequestDto request)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            
            var result = await vendedorCoordenadasService.GetListCoordenadasVendedorAsync(request);
            
            stopwatch.Stop();
            
            var response = ApiResponse<List<VendedorCoordenadasDto>>.SuccessResponse(result, "Los vendedores fueron obtenidos correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            
            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }
    
    [HttpGet ("obtenerGeocercasAsync")]
    public async Task<ActionResult<ApiResponse<PaginatedResultDto<GeocercaListDto>>>> GetAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? estado = null,
        [FromQuery] bool? activo = null)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            if (pageSize > 100)
            {
                throw new BadRequestException("El tamaño de la página no puede ser mayor a 100");
            }
            
            var result = await geocercaService.GetAllAsync(pageNumber, pageSize, searchTerm, estado, activo);
            
            stopwatch.Stop();
            
            var response = ApiResponse<PaginatedResultDto<GeocercaListDto>>.SuccessResponse(result, "Las geocercas fueron obtenidas correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            
            return Ok(response);

        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
        
    }
    
    [HttpGet ("obtenerGeocercasConVendedorAsync")]
    public async Task<ActionResult<ApiResponse<PaginatedResultDto<GeocercaConVendedorDto>>>> GetGeocercasConVendedorAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? estado = null,
        [FromQuery] bool? activo = null,
        [FromQuery] bool soloConVendedores = false)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            if (pageSize > 100)
            {
                throw new BadRequestException("El tamaño de la página no puede ser mayor a 100");
            }
            
            var result = await geocercaService.GetAllGeocercaConVendedorAsync(pageNumber, pageSize, searchTerm, estado, activo, soloConVendedores);
            
            stopwatch.Stop();
            
            var response = ApiResponse<PaginatedResultDto<GeocercaConVendedorDto>>.SuccessResponse(result, "Las geocercas fueron obtenidas correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            
            return Ok(response);

        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
        
    }
    
    [HttpPost("crear-con-vendedores")]
    public async Task<ActionResult<ApiResponse<GeocercaConVendedoresCreateResponseDto>>> CreateGeocercaConVendedoresAsync([FromBody]GeocercaConVendedoresCreateDto createDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            
            var result = await geocercaService.CreateGeocercaConVendedoresAsync(createDto);
            
            stopwatch.Stop();
            
            var response = ApiResponse<GeocercaConVendedoresCreateResponseDto>.SuccessResponse(result, "La geocerca fue creada correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            
            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
            
    }

}