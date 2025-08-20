using System.Text.Json.Serialization;

namespace ApiNetCore.Dtos;

public class UbicacionVendedorDto
{
    [JsonPropertyName("geubfech")]
    public DateTime Geubfech { get; set; }

    [JsonPropertyName("geublat")]
    public decimal Geublat { get; set; }

    [JsonPropertyName("geublon")]
    public decimal Geublon { get; set; }
}