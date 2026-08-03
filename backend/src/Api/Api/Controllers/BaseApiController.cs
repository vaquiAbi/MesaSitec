using System.Security.Claims;
using Api.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1")]
public abstract class BaseApiController : ControllerBase
{
    protected Guid UsuarioId
    {
        get
        {
            var val = User.FindFirst("sub")?.Value 
                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(val) || !Guid.TryParse(val, out var id))
            {
                throw ExcepcionApi.NoAutenticado("Token ausente, inválido o expirado.");
            }

            return id;
        }
    }

    protected Guid TenantId
    {
        get
        {
            var val = User.FindFirst("tenantId")?.Value 
                   ?? User.FindFirst(ClaimTypes.GroupSid)?.Value;

            if (string.IsNullOrEmpty(val) || !Guid.TryParse(val, out var id))
            {
                throw ExcepcionApi.NoAutenticado("Token ausente, inválido o expirado.");
            }

            return id;
        }
    }

    protected string Rol
    {
        get
        {
            var val = User.FindFirst("rol")?.Value 
                   ?? User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(val))
            {
                throw ExcepcionApi.NoAutenticado("Token ausente, inválido o expirado.");
            }

            return val;
        }
    }

    protected string Email
    {
        get
        {
            var val = User.FindFirst("email")?.Value 
                   ?? User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(val))
            {
                throw ExcepcionApi.NoAutenticado("Token ausente, inválido o expirado.");
            }

            return val;
        }
    }
}
