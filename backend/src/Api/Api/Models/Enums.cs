using System.Text.Json.Serialization;

namespace Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RolUsuario
{
    Admin,
    Agente,
    Solicitante
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PrioridadSolicitud
{
    Baja,
    Media,
    Alta,
    Critica
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EstadoSolicitud
{
    Nueva,
    Asignada,
    EnProceso,
    Resuelta,
    Cerrada,
    Cancelada
}
