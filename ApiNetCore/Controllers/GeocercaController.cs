using System.Diagnostics;
using ApiNetCore.Dtos;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Dtos.Vendedor.Listas;
using ApiNetCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GeocercaController(IGeocercaService geocercaService) : ControllerBase
{
    [HttpPost("crear")]
    public async Task<ActionResult<ApiResponse<CreateGeocercaDto>>> CreateGeocerca([FromBody] CreateGeocercaDto createGeocercaDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.CreateGeocerca(createGeocercaDto);
            stopwatch.Stop();
            
            var response = ApiResponse<CreateGeocercaDto>.SuccessResponse(result, "Geocerca creada exitosamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("crear-para-vendedor/{idVendedor}")]
    public async Task<ActionResult<ApiResponse<CreateGeocercaVendedorDto>>> CreateGeocercaForVendedor(
        int idVendedor, 
        [FromBody] CreateGeocercaVendedorDto createGeocercaVendedorDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.CreateGeocercaForVendedor(idVendedor, createGeocercaVendedorDto);
            stopwatch.Stop();
            
            var response = ApiResponse<CreateGeocercaVendedorDto>.SuccessResponse(result, "Geocerca creada y asignada al vendedor exitosamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    // ✅ MODIFICADO: Actualizar recibe ID como parámetro de ruta
    [HttpPut("actualizar/{idGeocerca}")]
    public async Task<ActionResult<ApiResponse<UpdateGeocercaDto>>> UpdateGeocerca(
        int idGeocerca, 
        [FromBody] UpdateGeocercaDto updateGeocercaDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.UpdateGeocerca(idGeocerca, updateGeocercaDto);
            stopwatch.Stop();
            
            var response = ApiResponse<UpdateGeocercaDto>.SuccessResponse(result, "Geocerca actualizada exitosamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("eliminar/{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteGeocerca(int id)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await geocercaService.DeleteGeocerca(id);
            stopwatch.Stop();
            
            var response = ApiResponse<bool>.SuccessResponse(result, "Geocerca eliminada exitosamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("paginados")]
    public async Task<ActionResult<ApiResponse<LGeocercaDto>>> GetGeocercas(int pagina = 1, int tamanioPagina = 10)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            
            if (pagina < 1)
                return BadRequest("La página debe ser un número entero mayor a 0");
            if (tamanioPagina is < 1 or > 100)
                return BadRequest("El tamaño de página debe estar entre 1 y 100");
            
            var geocercas = await geocercaService.GetGeocercasAsync(pagina, tamanioPagina);
            stopwatch.Stop();
            
            var response = ApiResponse<LGeocercaDto>.SuccessResponse(geocercas, "Geocercas obtenidas exitosamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("obtener/{id}")]
    public async Task<ActionResult<ApiResponse<GeocercaConVendedorDto>>> GetGeocercaById(int id)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var geocerca = await geocercaService.GetGeocercaByIdAsync(id);
            stopwatch.Stop();
            
            var response = ApiResponse<GeocercaConVendedorDto>.SuccessResponse(geocerca, "Geocerca obtenida exitosamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}