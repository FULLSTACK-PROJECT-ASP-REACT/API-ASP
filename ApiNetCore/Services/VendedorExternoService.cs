using System.Text.Json;
using ApiNetCore.Dtos;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;

namespace ApiNetCore.Services;

public class VendedorExternoService : IVendedorExternoService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VendedorExternoService> _logger;
    private readonly IConfiguration _configuration;

    public VendedorExternoService(HttpClient httpClient, ILogger<VendedorExternoService> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }


    public async Task<List<VendedorExternoDto>> GetVendedoresAsync(string token)
    {
            if (string.IsNullOrEmpty(token))
                throw new BadRequestException("Token de API externo es requerido");
            try
            {

                var baseUrl = _configuration["ApiExterno:BaseUrl"] ?? "https://main.egasyasociados.com:4041";

                var requestUri = $"{baseUrl}/api/usuarios/listacompleta";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                _logger.LogInformation("Consultando API externo: {RequestUri}", requestUri);

                var response = await _httpClient.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error en API externo. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, errorContent);

                    throw new InternalServerException(
                        $"Error al consultar API externo. Status: {response.StatusCode}");
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(jsonContent))
                    return [];

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var vendedores = JsonSerializer.Deserialize<List<VendedorExternoDto>>(jsonContent, options);

                _logger.LogInformation("API externo consultado exitosamente. Vendedores obtenidos: {Count}",
                    vendedores?.Count ?? 0);

                return vendedores ?? [];
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión con API externo");
                throw new InternalServerException($"Error de conexión con API externo: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout en consulta a API externo");
                throw new InternalServerException("Timeout en consulta a API externo");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al deserializar respuesta de API externo");
                throw new InternalServerException($"Error al procesar respuesta de API externo: {ex.Message}");
            }
            catch (Exception ex) when (ex is not (BadRequestException or InternalServerException))
            {
                _logger.LogError(ex, "Error inesperado en consulta a API externo");
                throw new InternalServerException($"Error inesperado al consultar API externo: {ex.Message}");
            }
    }
}