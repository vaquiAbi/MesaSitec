using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Modelos.Dto.Request;

public class UsuarioRequestDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es obligatorio.")]
    public RolUsuario Rol { get; set; }

    public Guid TenantId { get; set; }
}
