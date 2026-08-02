using System.ComponentModel.DataAnnotations;
using Api.Models;

namespace Api.Modelos.Dto.Request;

public class CrearSolicitudRequestDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(120, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 120 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(4000, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 4000 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public Guid CategoriaId { get; set; }

    [Required(ErrorMessage = "La prioridad es obligatoria.")]
    public PrioridadSolicitud Prioridad { get; set; }
}
