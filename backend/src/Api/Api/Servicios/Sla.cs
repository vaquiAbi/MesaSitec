using Api.Models;

namespace Api.Servicios;

public static class Sla
{
  
    // fechaLimiteSla = fechaCreacion + (categoria.slaHoras * factor[prioridad])
    // Factores: Critica: 0.5, Alta: 0.75, Media: 1.0, Baja: 2.0
 
    public static DateTime CalcularFechaLimite(DateTime fechaCreacion, int slaHoras, PrioridadSolicitud prioridad)
    {
        double factor = prioridad switch
        {
            PrioridadSolicitud.Critica => 0.5,
            PrioridadSolicitud.Alta => 0.75,
            PrioridadSolicitud.Media => 1.0,
            PrioridadSolicitud.Baja => 2.0,
            _ => 1.0
        };

        return fechaCreacion.AddHours(slaHoras * factor);
    }

    // Se considera vencida si fechaLimiteSla ya pasó y su estado no es Resuelta, Cerrada ni Cancelada.
    
    public static bool EsVencida(DateTime fechaLimiteSla, EstadoSolicitud estado, DateTime fechaReferencia)
    {
        if (estado == EstadoSolicitud.Resuelta ||
            estado == EstadoSolicitud.Cerrada ||
            estado == EstadoSolicitud.Cancelada)
        {
            return false;
        }

        return fechaLimiteSla < fechaReferencia;
    }
}
