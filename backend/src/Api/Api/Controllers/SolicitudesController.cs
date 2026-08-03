using Api.Modelos.Dto.Request;
using Api.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class SolicitudesController : BaseApiController
{
    private readonly SolicitudServicio _solicitudServicio;

    public SolicitudesController(SolicitudServicio solicitudServicio)
    {
        _solicitudServicio = solicitudServicio;
    }

    [HttpGet("solicitudes")]
    public async Task<IActionResult> ObtenerSolicitudes([FromQuery] SolicitudesQueryParamsDto queryParams)
    {
        var resultado = await _solicitudServicio.ObtenerListadoPaginadoAsync(TenantId, UsuarioId, Rol, queryParams);
        return Ok(resultado);
    }
}
