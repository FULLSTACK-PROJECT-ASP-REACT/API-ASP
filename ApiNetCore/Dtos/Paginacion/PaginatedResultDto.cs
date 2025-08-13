namespace ApiNetCore.Dtos.Paginacion;

public class PaginatedResultDto<T>
{
    public List<T> Data { get; set; } = new List<T>();
    public PaginacionDto Paginacion { get; set; } = null!;
}