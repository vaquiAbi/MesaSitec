namespace Api.Modelos.Dto.Response;

public class ErrorDetalleDto
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errores { get; set; }
}
