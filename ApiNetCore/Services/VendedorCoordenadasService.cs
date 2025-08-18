using System.Net;
using System.Text.Json;
using ApiNetCore.ContextMysql;
using ApiNetCore.Dtos.Usuario;
using ApiNetCore.Exceptions;
using ApiNetCore.Models;
using ApiNetCore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ApiNetCore.Services;

public class VendedorCoordenadasService(
    MyDbContextMysql dbContextMysql,
    HttpClient httpClient,
    ILogger<VendedorCoordenadasService> logger)
    : IVendedorCoordenadasService
{
    public async Task<List<VendedorCoordenadasDto>> GetListCoordenadasVendedorAsync(GetListCoordenadasVendedorRequestDto request)
    {
        try
        {
            var usuarios = await ObtenerUsuariosWebServiceAsync(request.Token);

            // 2. Filtrar usuarios según criterios
            var usuariosFiltrados = FiltrarUsuarios(usuarios, request);

            // 3. Obtener ubicaciones de la BD
            var ubicaciones = await ObtenerUbicacionesBdAsync(usuariosFiltrados, request);

            // 4. Combinar datos y crear respuesta
            var resultado = CombinarDatos(usuariosFiltrados, ubicaciones);
            
            return resultado;
            

        }
        catch (Exception ex) when (ex is not (not BadRequestException or NotFoundException or UnauthorizedException or ForbiddenException or InternalServerException))
        {
            throw new InternalServerException($"Error al obtener la geocerca: {ex.Message}");
        }
    }
    
    private async Task<List<UsuarioWebServiceDto>> ObtenerUsuariosWebServiceAsync(string token)
    {
        const string webServiceUrl = "https://main.egasyasociados.com:4041/api/usuarios/listacompleta";

        using var request = new HttpRequestMessage(HttpMethod.Get, webServiceUrl);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.LogError("Error en web service. Status: {StatusCode}, Content: {Content}", 
                response.StatusCode, errorContent);
            
            throw response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => new UnauthorizedException("Token de autorización inválido"),
                HttpStatusCode.Forbidden => new ForbiddenException("No tiene permisos para acceder al servicio"),
                HttpStatusCode.NotFound => new NotFoundException("Servicio no encontrado"),
                _ => new InternalServerException($"Error en servicio externo: {response.StatusCode}")
            };
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        var usuarios = JsonSerializer.Deserialize<List<UsuarioWebServiceDto>>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return usuarios ?? [];
    }

    private static List<UsuarioWebServiceDto> FiltrarUsuarios(List<UsuarioWebServiceDto> usuarios, GetListCoordenadasVendedorRequestDto request)
    {
        var query = usuarios.AsQueryable();

        if (request.SoloConGeolocalizacion == true)
        {
            query = query.Where(u => u.Usugeol);
        }

        if (request.SoloConAccesoApp == true)
        {
            query = query.Where(u => u.Usuapp);
        }

        return query.ToList();
    }

    private async Task<List<Geoubi>> ObtenerUbicacionesBdAsync(List<UsuarioWebServiceDto> usuarios, GetListCoordenadasVendedorRequestDto request)
    {
        var codigosUsuarios = usuarios.Select(u => u.Usucod).ToList();

        logger.LogInformation("Buscando ubicaciones para usuarios: {Usuarios}", string.Join(", ", codigosUsuarios));

        var todasLasUbicaciones = await dbContextMysql.Geoubis
            .Take(10) // Solo las primeras 10 para debug
            .ToListAsync();

        logger.LogInformation("Muestra de ubicaciones en BD: {Ubicaciones}", 
            string.Join(", ", todasLasUbicaciones.Select(u => $"'{u.Geubusu}'")));

        // Hacer la consulta con Trim para eliminar espacios y comparación insensible a mayúsculas
        var query = dbContextMysql.Geoubis
            .Where(g => codigosUsuarios.Contains(g.Geubusu.Trim()));

        if (request.FechaDesde.HasValue)
        {
            query = query.Where(g => g.Geubfech >= request.FechaDesde.Value);
        }

        if (request.FechaHasta.HasValue)
        {
            query = query.Where(g => g.Geubfech <= request.FechaHasta.Value);
        }

        var ubicacionesEncontradas = await query
            .OrderBy(g => g.Geubusu)
            .ThenByDescending(g => g.Geubfech)
            .ToListAsync();

        logger.LogInformation("Ubicaciones encontradas: {Count} para usuarios: {UsuariosEncontrados}", 
            ubicacionesEncontradas.Count, 
            string.Join(", ", ubicacionesEncontradas.Select(u => u.Geubusu).Distinct()));

        return ubicacionesEncontradas;
    }

    private static List<VendedorCoordenadasDto> CombinarDatos(List<UsuarioWebServiceDto> usuarios, List<Geoubi> ubicaciones)
    {
        var resultado = (from usuario in usuarios
            let ubicacionesUsuario = ubicaciones.Where(u => u.Geubusu == usuario.Usucod)
                .Select(u => new UbicacionDto { Id = u.Geubid, Fecha = u.Geubfech, Latitud = u.Geublat, Longitud = u.Geublon })
                .OrderByDescending(u => u.Fecha)
                .ToList()
            select new VendedorCoordenadasDto
            {
                CodigoUsuario = usuario.Usucod,
                NombreUsuario = usuario.Usunombre,
                Email = usuario.Usuemail,
                TieneAccesoApp = usuario.Usuapp,
                TieneAccesoWebApp = usuario.Usuwebapp,
                TieneAccesoGeolocalizacion = usuario.Usugeol,
                Ubicaciones = ubicacionesUsuario,
                TotalUbicaciones = ubicacionesUsuario.Count,
                UltimaUbicacion = ubicacionesUsuario.FirstOrDefault()?.Fecha
            }).ToList();

        return resultado.OrderBy(v => v.NombreUsuario).ToList();
    }
}