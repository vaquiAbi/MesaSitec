namespace Api.Models;

public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
    public bool Activo { get; set; } = true;


    public Tenant? Tenant { get; set; }
    public ICollection<Solicitud> SolicitudesCreadas { get; set; } = new List<Solicitud>();
    public ICollection<Solicitud> SolicitudesAsignadas { get; set; } = new List<Solicitud>();
}
