namespace ApiNetCore.Dtos.Usuario;

public class UbicacionDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
}