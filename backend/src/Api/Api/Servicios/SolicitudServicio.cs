using Api.Excepciones;
using Api.Infraestructura;
using Api.Modelos.Dto.Request;
using Api.Modelos.Dto.Response;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Servicios;

public class SolicitudServicio
{
    private readonly MesaSitecDbContext _db;

    public SolicitudServicio(MesaSitecDbContext db)
    {
        _db = db;
    }

    public async Task<PaginacionResponseDto<SolicitudItemDto>> ObtenerListadoPaginadoAsync(
        Guid tenantId,
        Guid usuarioId,
        string rol,
        SolicitudesQueryParamsDto queryParams)
    {
        if (queryParams.Page < 1 || queryParams.PageSize > 100)
        {
            throw ExcepcionApi.ParametroInvalido("Parámetro de consulta fuera de rango.");
        }

        var query = _db.Solicitudes
            .Include(s => s.Categoria)
            .Include(s => s.Agente)
            .Where(s => s.TenantId == tenantId); 
     
        if (rol == "Solicitante")
        {
            query = query.Where(s => s.SolicitanteId == usuarioId);
        }

        
        if (queryParams.Estado.HasValue)
        {
            query = query.Where(s => s.Estado == queryParams.Estado.Value);
        }

        if (queryParams.Prioridad.HasValue)
        {
            query = query.Where(s => s.Prioridad == queryParams.Prioridad.Value);
        }

        if (queryParams.CategoriaId.HasValue)
        {
            query = query.Where(s => s.CategoriaId == queryParams.CategoriaId.Value);
        }

        if (queryParams.AgenteId.HasValue)
        {
            query = query.Where(s => s.AgenteId == queryParams.AgenteId.Value);
        }

        if (!string.IsNullOrWhiteSpace(queryParams.Q))
        {
            var q = queryParams.Q.Trim().ToLower();
            query = query.Where(s =>
                s.Titulo.ToLower().Contains(q) ||
                s.Descripcion.ToLower().Contains(q) ||
                s.Codigo.ToLower().Contains(q));
        }

        if (queryParams.Vencidas.HasValue && queryParams.Vencidas.Value)
        {
            var ahora = DateTime.UtcNow;
            query = query.Where(s =>
                s.FechaLimiteSla < ahora &&
                s.Estado != EstadoSolicitud.Resuelta &&
                s.Estado != EstadoSolicitud.Cerrada &&
                s.Estado != EstadoSolicitud.Cancelada);
        }

  
        var sort = queryParams.Sort?.Trim() ?? "-fechaCreacion";
        query = sort switch
        {
            "fechaCreacion" => query.OrderBy(s => s.FechaCreacion),
            "-fechaCreacion" => query.OrderByDescending(s => s.FechaCreacion),
            "prioridad" => query.OrderBy(s => 
                s.Prioridad == PrioridadSolicitud.Baja ? 1 :
                s.Prioridad == PrioridadSolicitud.Media ? 2 :
                s.Prioridad == PrioridadSolicitud.Alta ? 3 : 4),
            "-prioridad" => query.OrderByDescending(s => 
                s.Prioridad == PrioridadSolicitud.Baja ? 1 :
                s.Prioridad == PrioridadSolicitud.Media ? 2 :
                s.Prioridad == PrioridadSolicitud.Alta ? 3 : 4),
            "codigo" => query.OrderBy(s => s.Codigo),
            "-codigo" => query.OrderByDescending(s => s.Codigo),
            _ => query.OrderByDescending(s => s.FechaCreacion)
        };

        var total = await query.CountAsync();
        var totalPaginas = (int)Math.Ceiling(total / (double)queryParams.PageSize);

        var ahoraUtc = DateTime.UtcNow;

        var items = await query
            .Skip((queryParams.Page - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(s => new SolicitudItemDto
            {
                Id = s.Id,
                Codigo = s.Codigo,
                Titulo = s.Titulo,
                Estado = s.Estado.ToString(),
                Prioridad = s.Prioridad.ToString(),
                Categoria = new CategoriaDto
                {
                    Id = s.Categoria.Id,
                    Nombre = s.Categoria.Nombre,
                    SlaHoras = s.Categoria.SlaHoras
                },
                Agente = s.Agente != null ? new UsuarioResumenDto
                {
                    Id = s.Agente.Id,
                    Nombre = s.Agente.Nombre
                } : null,
                FechaCreacion = s.FechaCreacion,
                FechaLimiteSla = s.FechaLimiteSla,
                Vencida = s.FechaLimiteSla < ahoraUtc &&
                          s.Estado != EstadoSolicitud.Resuelta &&
                          s.Estado != EstadoSolicitud.Cerrada &&
                          s.Estado != EstadoSolicitud.Cancelada
            })
            .ToListAsync();

        return new PaginacionResponseDto<SolicitudItemDto>
        {
            Items = items,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize,
            Total = total,
            TotalPaginas = totalPaginas
        };
    }
}
