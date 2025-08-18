namespace ApiNetCore.Dtos.Usuario;

public class UsuarioWebServiceDto
{
    public string Usucod { get; set; } = null!;
    public string Usuclave { get; set; } = null!;
    public string Usunombre { get; set; } = null!;
    public string Usuemail { get; set; } = null!;
    public bool Usuapp { get; set; }
    public bool Usuwebapp { get; set; }
    public bool Usugeol { get; set; }
}