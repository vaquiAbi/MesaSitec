using System.ComponentModel.DataAnnotations;

namespace Api.Modelos.Dto.Request;

public class TransicionSolicitudRequestDto
{
    [Required(ErrorMessage = "La acción es obligatoria.")]
    public string Accion { get; set; } = string.Empty;

    public Guid? AgenteId { get; set; }

    public string? Motivo { get; set; }
}
