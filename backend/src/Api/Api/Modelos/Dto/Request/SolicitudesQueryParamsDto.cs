using Api.Models;

namespace Api.Modelos.Dto.Request;

public class SolicitudesQueryParamsDto
{
    public EstadoSolicitud? Estado { get; set; }
    public PrioridadSolicitud? Prioridad { get; set; }
    public Guid? CategoriaId { get; set; }
    public Guid? AgenteId { get; set; }
    public string? Q { get; set; }//busca en titulo, descripcion y codigo, sin distinguir mayúsculas
    public bool? Vencidas { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string Sort { get; set; } = "-fechaCreacion";
}
