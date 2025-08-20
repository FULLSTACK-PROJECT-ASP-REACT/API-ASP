using System.Text.Json.Serialization;

namespace ApiNetCore.Dtos;

public class VendedorExternoDto
{
    [JsonPropertyName("usucod")]
    public string Usucod { get; set; } = string.Empty;

    [JsonPropertyName("usunombre")]
    public string Usunombre { get; set; } = string.Empty;

    [JsonPropertyName("usuemail")]
    public string Usuemail { get; set; } = string.Empty;

    [JsonPropertyName("usucodv")]
    public string Usucodv { get; set; } = string.Empty;

    [JsonPropertyName("ubicacion")]
    public UbicacionVendedorDto? Ubicacion { get; set; }
}