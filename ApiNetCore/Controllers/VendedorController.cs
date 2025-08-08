using System.Diagnostics;
using ApiNetCore.Dtos;

using ApiNetCore.Dtos.Vendedor;
using ApiNetCore.Dtos.Vendedor.Listas;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VendedorController(IVendedorService vendedorService) : ControllerBase
{
    [HttpPost("CrearVendedor")]
    public async Task<ActionResult<ApiResponse<CreateVendedorDto>>> CreateVendedor([FromBody] CreateVendedorDto createVendedorDto)
    {
        var stopwatch = Stopwatch.StartNew();
        var vendedor = await vendedorService.CreateVendedor(createVendedorDto);
        stopwatch.Stop();
        
        var response = ApiResponse<CreateVendedorDto>.SuccessResponse(vendedor, "Vendedor creado exitosamente");
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        return Ok(response);
    }
    [HttpGet("paginados")]
    public async Task<ActionResult<ApiResponse<LVendedorDto>>> GetVendedores(int pagina = 1, int tamanioPagina = 10)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
        
            if (pagina < 1)
                throw new BadRequestException("La página debe ser un número entero mayor a 0");
        
            if (tamanioPagina is < 1 or > 100)
                return BadRequest("El tamaño de página debe estar entre 1 y 100");
        
            var vendedores = await vendedorService.GetVendedoresAsync(pagina, tamanioPagina);
            stopwatch.Stop();
        
            var response = ApiResponse<LVendedorDto>.SuccessResponse(vendedores, "Vendedores obtenidos exitosamente");
            response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        
            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
    
}