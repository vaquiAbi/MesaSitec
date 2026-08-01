namespace Api.Modelos.Dto.Response;

public class CategoriaDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int SlaHoras { get; set; }
}
