using Api.Models;
using Api.Servicios;
using Microsoft.EntityFrameworkCore;

namespace Api.Infraestructura;

public static class DbSembrar
{
    public static async Task SembrarAsync(MesaSitecDbContext dbContext, IConfiguration configuration)
    {
        
        if (await dbContext.Tenants.AnyAsync())
        {
            return;
        }

     
        var fechaBaseConfig = configuration["SEED_FECHA_BASE"]
                              ?? throw new InvalidOperationException("La variable de entorno 'SEED_FECHA_BASE' es requerida para datos de prueba.");

        if (!DateTime.TryParse(fechaBaseConfig, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var fechaBase))
        {
            throw new InvalidOperationException("El valor de 'SEED_FECHA_BASE' no representa una fecha válida.");
        }

        fechaBase = DateTime.SpecifyKind(fechaBase, DateTimeKind.Utc);

        
        var seedPassword = configuration["SEED_PASSWORD"]
                           ?? throw new InvalidOperationException("La variable de entorno 'SEED_PASSWORD' es requerida para el seeder.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(seedPassword);

        
        
        var tenantNorteId = configuration["TENANT_NORTE_ID"] ?? throw new InvalidOperationException("La variable 'TENANT_NORTE_ID' es requerida.");
        var tenantNorteNombre = configuration["TENANT_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable 'TENANT_NORTE_NOMBRE' es requerida.");
        var tenantSurId = configuration["TENANT_SUR_ID"] ?? throw new InvalidOperationException("La variable 'TENANT_SUR_ID' es requerida.");
        var tenantSurNombre = configuration["TENANT_SUR_NOMBRE"] ?? throw new InvalidOperationException("La variable 'TENANT_SUR_NOMBRE' es requerida.");

        var tenantNorte = new Tenant
        {
            Id = Guid.Parse(tenantNorteId),
            Nombre = tenantNorteNombre,
            Activo = true
        };

        var tenantSur = new Tenant
        {
            Id = Guid.Parse(tenantSurId),
            Nombre = tenantSurNombre,
            Activo = true
        };

        await dbContext.Tenants.AddRangeAsync(tenantNorte, tenantSur);

     
        var adminNorte = new Usuario
        {
            Id = Guid.Parse(configuration["USER_ADMIN_NORTE_ID"] ?? throw new InvalidOperationException("La variable USER_ADMIN_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Email = configuration["USER_ADMIN_NORTE_EMAIL"] ?? throw new InvalidOperationException("La variable USER_ADMIN_NORTE_EMAIL es requerida."),
            PasswordHash = passwordHash,
            Nombre = configuration["USER_ADMIN_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable USER_ADMIN_NORTE_NOMBRE es requerida."),
            Rol = RolUsuario.Admin,
            Activo = true
        };

        var agente1Norte = new Usuario
        {
            Id = Guid.Parse(configuration["USER_AGENTE1_NORTE_ID"] ?? throw new InvalidOperationException("La variable USER_AGENTE1_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Email = configuration["USER_AGENTE1_NORTE_EMAIL"] ?? throw new InvalidOperationException("La variable USER_AGENTE1_NORTE_EMAIL es requerida."),
            PasswordHash = passwordHash,
            Nombre = configuration["USER_AGENTE1_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable USER_AGENTE1_NORTE_NOMBRE es requerida."),
            Rol = RolUsuario.Agente,
            Activo = true
        };

        var agente2Norte = new Usuario
        {
            Id = Guid.Parse(configuration["USER_AGENTE2_NORTE_ID"] ?? throw new InvalidOperationException("La variable USER_AGENTE2_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Email = configuration["USER_AGENTE2_NORTE_EMAIL"] ?? throw new InvalidOperationException("La variable USER_AGENTE2_NORTE_EMAIL es requerida."),
            PasswordHash = passwordHash,
            Nombre = configuration["USER_AGENTE2_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable USER_AGENTE2_NORTE_NOMBRE es requerida."),
            Rol = RolUsuario.Agente,
            Activo = true
        };

        var user1Norte = new Usuario
        {
            Id = Guid.Parse(configuration["USER_USER1_NORTE_ID"] ?? throw new InvalidOperationException("La variable USER_USER1_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Email = configuration["USER_USER1_NORTE_EMAIL"] ?? throw new InvalidOperationException("La variable USER_USER1_NORTE_EMAIL es requerida."),
            PasswordHash = passwordHash,
            Nombre = configuration["USER_USER1_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable USER_USER1_NORTE_NOMBRE es requerida."),
            Rol = RolUsuario.Solicitante,
            Activo = true
        };

        var user2Norte = new Usuario
        {
            Id = Guid.Parse(configuration["USER_USER2_NORTE_ID"] ?? throw new InvalidOperationException("La variable USER_USER2_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Email = configuration["USER_USER2_NORTE_EMAIL"] ?? throw new InvalidOperationException("La variable USER_USER2_NORTE_EMAIL es requerida."),
            PasswordHash = passwordHash,
            Nombre = configuration["USER_USER2_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable USER_USER2_NORTE_NOMBRE es requerida."),
            Rol = RolUsuario.Solicitante,
            Activo = true
        };

        var adminSur = new Usuario
        {
            Id = Guid.Parse(configuration["USER_ADMIN_SUR_ID"] ?? throw new InvalidOperationException("La variable USER_ADMIN_SUR_ID es requerida.")),
            TenantId = tenantSur.Id,
            Email = configuration["USER_ADMIN_SUR_EMAIL"] ?? throw new InvalidOperationException("La variable USER_ADMIN_SUR_EMAIL es requerida."),
            PasswordHash = passwordHash,
            Nombre = configuration["USER_ADMIN_SUR_NOMBRE"] ?? throw new InvalidOperationException("La variable USER_ADMIN_SUR_NOMBRE es requerida."),
            Rol = RolUsuario.Admin,
            Activo = true
        };

        var user1Sur = new Usuario
        {
            Id = Guid.Parse(configuration["USER_USER1_SUR_ID"] ?? throw new InvalidOperationException("La variable USER_USER1_SUR_ID es requerida.")),
            TenantId = tenantSur.Id,
            Email = configuration["USER_USER1_SUR_EMAIL"] ?? throw new InvalidOperationException("La variable USER_USER1_SUR_EMAIL es requerida."),
            PasswordHash = passwordHash,
            Nombre = configuration["USER_USER1_SUR_NOMBRE"] ?? throw new InvalidOperationException("La variable USER_USER1_SUR_NOMBRE es requerida."),
            Rol = RolUsuario.Solicitante,
            Activo = true
        };

        await dbContext.Usuarios.AddRangeAsync(
            adminNorte, agente1Norte, agente2Norte, user1Norte, user2Norte,
            adminSur, user1Sur
        );

        // 6. Crear Categorías SLA para ambos tenants desde configuración
        var catIncidenteNorte = new Categoria
        {
            Id = Guid.Parse(configuration["CAT_INCIDENTE_NORTE_ID"] ?? throw new InvalidOperationException("La variable CAT_INCIDENTE_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Nombre = configuration["CAT_INCIDENTE_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable CAT_INCIDENTE_NORTE_NOMBRE es requerida."),
            SlaHoras = int.Parse(configuration["CAT_INCIDENTE_NORTE_SLA"] ?? "8"),
            Activo = true
        };

        var catRequerimientoNorte = new Categoria
        {
            Id = Guid.Parse(configuration["CAT_REQUERIMIENTO_NORTE_ID"] ?? throw new InvalidOperationException("La variable CAT_REQUERIMIENTO_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Nombre = configuration["CAT_REQUERIMIENTO_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable CAT_REQUERIMIENTO_NORTE_NOMBRE es requerida."),
            SlaHoras = int.Parse(configuration["CAT_REQUERIMIENTO_NORTE_SLA"] ?? "40"),
            Activo = true
        };

        var catConsultaNorte = new Categoria
        {
            Id = Guid.Parse(configuration["CAT_CONSULTA_NORTE_ID"] ?? throw new InvalidOperationException("La variable CAT_CONSULTA_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Nombre = configuration["CAT_CONSULTA_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable CAT_CONSULTA_NORTE_NOMBRE es requerida."),
            SlaHoras = int.Parse(configuration["CAT_CONSULTA_NORTE_SLA"] ?? "24"),
            Activo = true
        };

        var catCriticaNorte = new Categoria
        {
            Id = Guid.Parse(configuration["CAT_CRITICA_NORTE_ID"] ?? throw new InvalidOperationException("La variable CAT_CRITICA_NORTE_ID es requerida.")),
            TenantId = tenantNorte.Id,
            Nombre = configuration["CAT_CRITICA_NORTE_NOMBRE"] ?? throw new InvalidOperationException("La variable CAT_CRITICA_NORTE_NOMBRE es requerida."),
            SlaHoras = int.Parse(configuration["CAT_CRITICA_NORTE_SLA"] ?? "4"),
            Activo = true
        };

        var catIncidenteSur = new Categoria
        {
            Id = Guid.Parse(configuration["CAT_INCIDENTE_SUR_ID"] ?? throw new InvalidOperationException("La variable CAT_INCIDENTE_SUR_ID es requerida.")),
            TenantId = tenantSur.Id,
            Nombre = configuration["CAT_INCIDENTE_SUR_NOMBRE"] ?? throw new InvalidOperationException("La variable CAT_INCIDENTE_SUR_NOMBRE es requerida."),
            SlaHoras = int.Parse(configuration["CAT_INCIDENTE_SUR_SLA"] ?? "8"),
            Activo = true
        };

        var catRequerimientoSur = new Categoria
        {
            Id = Guid.Parse(configuration["CAT_REQUERIMIENTO_SUR_ID"] ?? throw new InvalidOperationException("La variable CAT_REQUERIMIENTO_SUR_ID es requerida.")),
            TenantId = tenantSur.Id,
            Nombre = configuration["CAT_REQUERIMIENTO_SUR_NOMBRE"] ?? throw new InvalidOperationException("La variable CAT_REQUERIMIENTO_SUR_NOMBRE es requerida."),
            SlaHoras = int.Parse(configuration["CAT_REQUERIMIENTO_SUR_SLA"] ?? "40"),
            Activo = true
        };

        var catConsultaSur = new Categoria
        {
            Id = Guid.Parse(configuration["CAT_CONSULTA_SUR_ID"] ?? throw new InvalidOperationException("La variable CAT_CONSULTA_SUR_ID es requerida.")),
            TenantId = tenantSur.Id,
            Nombre = configuration["CAT_CONSULTA_SUR_NOMBRE"] ?? throw new InvalidOperationException("La variable CAT_CONSULTA_SUR_NOMBRE es requerida."),
            SlaHoras = int.Parse(configuration["CAT_CONSULTA_SUR_SLA"] ?? "24"),
            Activo = true
        };

        var catCriticaSur = new Categoria
        {
            Id = Guid.Parse(configuration["CAT_CRITICA_SUR_ID"] ?? throw new InvalidOperationException("La variable CAT_CRITICA_SUR_ID es requerida.")),
            TenantId = tenantSur.Id,
            Nombre = configuration["CAT_CRITICA_SUR_NOMBRE"] ?? throw new InvalidOperationException("La variable CAT_CRITICA_SUR_NOMBRE es requerida."),
            SlaHoras = int.Parse(configuration["CAT_CRITICA_SUR_SLA"] ?? "4"),
            Activo = true
        };

        await dbContext.Categorias.AddRangeAsync(
            catIncidenteNorte, catRequerimientoNorte, catConsultaNorte, catCriticaNorte,
            catIncidenteSur, catRequerimientoSur, catConsultaSur, catCriticaSur
        );

        

        //  25 Solicitudes  Cooperativa Norte
        var solicitudesNorte = new List<Solicitud>();
        var categoriasNorte = new[] { catIncidenteNorte, catRequerimientoNorte, catConsultaNorte, catCriticaNorte };
        var prioridades = new[] { PrioridadSolicitud.Baja, PrioridadSolicitud.Media, PrioridadSolicitud.Alta, PrioridadSolicitud.Critica };
        var usuariosNorte = new[] { user1Norte, user2Norte };
        var agentesNorte = new[] { agente1Norte, agente2Norte };

        for (int i = 1; i <= 25; i++)
        {
            var cat = categoriasNorte[(i - 1) % categoriasNorte.Length];
            var prio = prioridades[(i - 1) % prioridades.Length];
            var solicitante = usuariosNorte[(i - 1) % usuariosNorte.Length];
            Usuario? agente = i % 2 == 0 ? agentesNorte[(i - 1) % agentesNorte.Length] : null;

            EstadoSolicitud estado;
            DateTime fechaCreacion;
            string? motivoRes = null;
            string? motivoCanc = null;
            DateTime? fechaRes = null;

            if (i <= 5)
            {
                // Solicitudes VENCIDAS
                fechaCreacion = fechaBase.AddDays(-4).AddHours(i);
                estado = (i % 2 == 0) ? EstadoSolicitud.EnProceso : EstadoSolicitud.Nueva;
                if (estado == EstadoSolicitud.EnProceso) agente = agentesNorte[0];
            }
            else if (i <= 9)
            {
                //  Solicitudes RESUELTAS / CERRADAS
                fechaCreacion = fechaBase.AddDays(-2).AddHours(i);
                estado = (i == 9) ? EstadoSolicitud.Cerrada : EstadoSolicitud.Resuelta;
                agente = agentesNorte[1];
                fechaRes = fechaCreacion.AddHours(3);
                motivoRes = "Se solucionó el inconveniente de acceso restableciendo las credenciales del usuario.";
            }
            else if (i <= 12)
            {
                //  Solicitudes CANCELADAS
                fechaCreacion = fechaBase.AddDays(-1).AddHours(i);
                estado = EstadoSolicitud.Cancelada;
                motivoCanc = "Solicitud duplicada ingresada por el usuario.";
            }
            else if (i <= 18)
            {
                //  Solicitudes ASIGNADAS
                fechaCreacion = fechaBase.AddHours(-(i * 2));
                estado = EstadoSolicitud.Asignada;
                agente = agentesNorte[i % 2];
            }
            else
            {
                //  Solicitudes NUEVAS / EN PROCESO VIGENTES
                fechaCreacion = fechaBase.AddHours(-i);
                estado = (i % 2 == 0) ? EstadoSolicitud.EnProceso : EstadoSolicitud.Nueva;
            }

            var slaLimit = Sla.CalcularFechaLimite(fechaCreacion, cat.SlaHoras, prio);

            solicitudesNorte.Add(new Solicitud
            {
                Id = Guid.NewGuid(),
                TenantId = tenantNorte.Id,
                Codigo = $"SOL-2026-{i:D5}",
                Titulo = $"Solicitud de soporte #{i} - {cat.Nombre}",
                Descripcion = $"Detalle descriptivo de la solicitud de soporte #{i} en Cooperativa Norte para la categoría {cat.Nombre}.",
                CategoriaId = cat.Id,
                Prioridad = prio,
                Estado = estado,
                SolicitanteId = solicitante.Id,
                AgenteId = agente?.Id,
                FechaCreacion = fechaCreacion,
                FechaLimiteSla = slaLimit,
                FechaResolucion = fechaRes,
                MotivoResolucion = motivoRes,
                MotivoCancelacion = motivoCanc
            });
        }

        await dbContext.Solicitudes.AddRangeAsync(solicitudesNorte);

        // 8 Solicitudes para Bufete Sur
        var solicitudesSur = new List<Solicitud>();
        var categoriasSur = new[] { catIncidenteSur, catRequerimientoSur, catConsultaSur, catCriticaSur };

        for (int i = 1; i <= 8; i++)
        {
            var cat = categoriasSur[(i - 1) % categoriasSur.Length];
            var prio = prioridades[(i - 1) % prioridades.Length];
            var fechaCreacion = fechaBase.AddHours(-(i * 3));
            var estado = (i % 3 == 0) ? EstadoSolicitud.Resuelta : (i % 2 == 0 ? EstadoSolicitud.Asignada : EstadoSolicitud.Nueva);

            string? motivoRes = estado == EstadoSolicitud.Resuelta ? "Se aplicó la solución técnica solicitada." : null;
            DateTime? fechaRes = estado == EstadoSolicitud.Resuelta ? fechaCreacion.AddHours(2) : null;

            var slaLimit = Sla.CalcularFechaLimite(fechaCreacion, cat.SlaHoras, prio);

            solicitudesSur.Add(new Solicitud
            {
                Id = Guid.NewGuid(),
                TenantId = tenantSur.Id,
                Codigo = $"SOL-2026-{i:D5}",
                Titulo = $"Ticket Bufete Sur #{i} - {cat.Nombre}",
                Descripcion = $"Detalle descriptivo del ticket #{i} para Bufete Sur en la categoría {cat.Nombre}.",
                CategoriaId = cat.Id,
                Prioridad = prio,
                Estado = estado,
                SolicitanteId = user1Sur.Id,
                AgenteId = null,
                FechaCreacion = fechaCreacion,
                FechaLimiteSla = slaLimit,
                FechaResolucion = fechaRes,
                MotivoResolucion = motivoRes
            });
        }

        await dbContext.Solicitudes.AddRangeAsync(solicitudesSur);

     
        await dbContext.SaveChangesAsync();
    }
}
