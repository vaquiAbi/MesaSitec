namespace Api.Models;

public class Categoria
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int SlaHoras { get; set; }
    public bool Activo { get; set; } = true;

    public Tenant? Tenant { get; set; }// N:1 Tenant
    public ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
}
