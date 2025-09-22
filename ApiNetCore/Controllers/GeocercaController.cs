using System.Diagnostics;
using ApiNetCore.Dtos;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Geocerca.GeoUsu;
using ApiNetCore.Dtos.Paginacion;
using ApiNetCore.Dtos.Vendedor;
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

    
    [HttpPatch("desvincular-vendedor/{codigo}")]
    public async Task<ActionResult<ApiResponse<GeocercaUpdateResponseDto>>> DesvincularVendedorAsync(string codigo)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.DesvincularVendedorAsync(codigo);
            stopwatch.Stop();

            var response =
                ApiResponse<GeocercaUpdateResponseDto>.SuccessResponse(result,
                    "El vendedor fue desvinculado correctamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
        
    }
    
    [HttpGet("getListGeofenceByEnterprise")]
    public async Task<ActionResult<ApiResponse<PaginatedResultDto<GeocercaListDto>>>> GetListGeofenceByEnterpriseAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string enterpriseName = "MEVECSA", 
        [FromQuery] bool? activo = null)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            if (pageSize > 100) throw new BadRequestException("El tamaño de la página no puede ser mayor a 100");

            var result = await geocercaService.GetListGeofenceByEnterpriseAsync(pageNumber, pageSize, activo, enterpriseName);

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
            [FromQuery] bool soloConVendedores = false,
            [FromQuery] string nameEnterprise = "MEVECSA")
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            if (pageSize > 100) throw new BadRequestException("El tamaño de la página no puede ser mayor a 100");

            var result = await geocercaService.GetAllGeocercaConVendedorAsync(pageNumber, pageSize, searchTerm, estado,
                activo, soloConVendedores, nameEnterprise);

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

    [HttpGet("ConsultarRelacion/{codigo}")]
    public async Task<ActionResult<ApiResponse<GeocercaUpdateResponseDto>>> ConsultarRelacionAsync(string codigo)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await geocercaService.ConsultarAsync(codigo);
        stopwatch.Stop();
        var response = ApiResponse<GeocercaUpdateResponseDto>.SuccessResponse(result,
            "La consulta se hizo correctamente");
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        return Ok(response);
    }
    
    [HttpPost("crear-geocercas")]
    public async Task<ActionResult<ApiResponse<GeocercaUpdateResponseDto>>> CreateGeocercaAsync([FromBody] GeocercaCreateDto createDto)
    {
        var stopwatch = Stopwatch.StartNew();

        var result = await geocercaService.CreateAsync(createDto);

        stopwatch.Stop();

        var response = ApiResponse<GeocercaUpdateResponseDto>.SuccessResponse(result,
            "La geocerca fue creada correctamente");
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

        return Ok(response);
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
    [HttpGet("validar-codigo-geocerca/{codigo}")]
    public async Task<ActionResult<ApiResponse<bool>>> ValidarCodigoGeocercaAsync([FromRoute] string codigo)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.ExistsAsync(codigo);
            stopwatch.Stop();
            
            if (result)
            {
                throw new BadRequestException($"El código de geocerca '{codigo}' ya existe y no se puede usar");
            }
            
            var response = ApiResponse<bool>.SuccessResponse(false, "El código de geocerca está disponible");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception ex) when (ex is not BadRequestException)
        {
            throw new InternalServerException(ex.Message, ex);
        }
    }
    
    [HttpPost("asignar-vendedor/{codigoGeocerca}")]
    public async Task<ActionResult<ApiResponse<VendorGeofenceAssignmentDto>>> AsignarVendedorAsync(string codigoGeocerca, [FromBody] VendorGeofenceAssignmentDto createDto)
    {
        var stopwatch = Stopwatch.StartNew();

        var result = await geocercaService.CreateGeocercaConVendedorAsync(createDto, codigoGeocerca);

        stopwatch.Stop();

        var response = ApiResponse<VendorGeofenceAssignmentDto>.SuccessResponse(result,
            $"Vendedor {createDto.Geugidv} asignado exitosamente a la geocerca {codigoGeocerca}");
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;

        return Ok(response);
    }
}