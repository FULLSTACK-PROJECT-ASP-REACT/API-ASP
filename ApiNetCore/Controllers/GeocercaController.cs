using System.Diagnostics;
using ApiNetCore.Dtos;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Geocerca.GeoUsu;
using ApiNetCore.Dtos.Paginacion;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class GeocercaController(IGeocercaService geocercaService) : ControllerBase
{
    
    [HttpDelete ("eliminar-geocerca/{codigo}")]
    public async Task<ActionResult<ApiResponse<GeocercaUpdateResponseDto>>> DeleteAsync(string codigo)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.DeleteAsync(codigo);
            stopwatch.Stop();
        
            var response = ApiResponse<GeocercaUpdateResponseDto>.SuccessResponse(result, "La geocerca fue eliminada correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }
    
    [HttpPatch ("activar-geocerca/{codigo}")]
    public async Task<ActionResult<ApiResponse<GeocercaUpdateResponseDto>>> ActivarAsync(string codigo)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.ActivarAsync(codigo);
            stopwatch.Stop();
        
            var response = ApiResponse<GeocercaUpdateResponseDto>.SuccessResponse(result, "La geocerca fue activada correctamente.");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }
    
    [HttpPatch ("desactivar-geocerca/{codigo}")]
    public async Task<ActionResult<ApiResponse<GeocercaUpdateResponseDto>>> DesactivarAsync(string codigo)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.DesactivarAsync(codigo);
            stopwatch.Stop();
        
            var response = ApiResponse<GeocercaUpdateResponseDto>.SuccessResponse(result, "La geocerca fue desactivada correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }
    
    [HttpPut("actualizar-geocerca/{codigo}")]
    public async Task<ActionResult<ApiResponse<GeocercaUpdateResponseDto>>> UpdateAsync(string codigo, GeocercaUpdateDto geocercaUpdateDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.UpdateAsync(codigo, geocercaUpdateDto);
            stopwatch.Stop();
        
            var response = ApiResponse<GeocercaUpdateResponseDto>.SuccessResponse(result, "La geocerca fue actualizada correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }
    [HttpGet("vendedores-con-geocercas")]
    public async Task<ActionResult<ApiResponse<PaginatedResultDto<GeocercaConVendedorDto>>>> GetVendedoresConGeocercas(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? activo = null,
        [FromQuery] string? estado = null)
    {
        try
        {
            var authHeader = Request.Headers.Authorization.FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
                return BadRequest(new { message = "Token de autorización requerido en el header Authorization" });

            var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? authHeader[7..]
                : authHeader;

            var result = await geocercaService.GetVendedoresConGeocercasAsync(
                token, pageNumber, pageSize, searchTerm, activo, estado);

            return Ok(result);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }


    [HttpGet("obtenerGeocercasAsync")]
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
            if (pageSize > 100) throw new BadRequestException("El tamaño de la página no puede ser mayor a 100");

            var result = await geocercaService.GetAllAsync(pageNumber, pageSize, searchTerm, estado, activo);

            stopwatch.Stop();

            var response =
                ApiResponse<PaginatedResultDto<GeocercaListDto>>.SuccessResponse(result,
                    "Las geocercas fueron obtenidas correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }

    [HttpGet("obtenerGeocercasConVendedorAsync")]
    public async Task<ActionResult<ApiResponse<PaginatedResultDto<GeocercaConVendedorDto>>>>
        GetGeocercasConVendedorAsync(
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
            if (pageSize > 100) throw new BadRequestException("El tamaño de la página no puede ser mayor a 100");

            var result = await geocercaService.GetAllGeocercaConVendedorAsync(pageNumber, pageSize, searchTerm, estado,
                activo, soloConVendedores);

            stopwatch.Stop();

            var response =
                ApiResponse<PaginatedResultDto<GeocercaConVendedorDto>>.SuccessResponse(result,
                    "Las geocercas fueron obtenidas correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }

    [HttpPost("crear-con-vendedores")]
    public async Task<ActionResult<ApiResponse<GeocercaConVendedoresCreateResponseDto>>>
        CreateGeocercaConVendedoresAsync([FromBody] GeocercaConVendedoresCreateDto createDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await geocercaService.CreateGeocercaConVendedoresAsync(createDto);

            stopwatch.Stop();

            var response =
                ApiResponse<GeocercaConVendedoresCreateResponseDto>.SuccessResponse(result,
                    "La geocerca fue creada correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }
}