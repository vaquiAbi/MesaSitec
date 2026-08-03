using System.Security.Claims;
using Api.Excepciones;
using Api.Infraestructura;
using Api.Modelos.Dto.Request;
using Api.Modelos.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/v1")]
public class AutController : ControllerBase
{
    private readonly MesaSitecDbContext _db;
    private readonly JwtTokenService _jwtTokenService;

    public AutController(MesaSitecDbContext db, JwtTokenService jwtTokenService)
    {
        _db = db;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw ExcepcionApi.NoAutenticado("Credenciales incorrectas.");
        }

        var usuario = await _db.Usuarios
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (usuario == null || !usuario.Activo)
        {
            throw ExcepcionApi.NoAutenticado("Credenciales incorrectas.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
        {
            throw ExcepcionApi.NoAutenticado("Credenciales incorrectas.");
        }

        var (token, expiraEn) = _jwtTokenService.GenerarToken(usuario);

        var usuarioDto = new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol.ToString(),
            TenantId = usuario.TenantId,
            TenantNombre = usuario.Tenant?.Nombre ?? string.Empty
        };

        return Ok(new LoginResponseDto
        {
            AccessToken = token,
            ExpiraEn = expiraEn,
            Usuario = usuarioDto
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> ObtenerPerfil()
    {
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(subClaim) || !Guid.TryParse(subClaim, out var usuarioId))
        {
            throw ExcepcionApi.NoAutenticado("Token ausente, inválido o expirado.");
        }

        var usuario = await _db.Usuarios
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.Id == usuarioId);

        if (usuario == null || !usuario.Activo)
        {
            throw ExcepcionApi.NoAutenticado("Token ausente, inválido o expirado.");
        }

        var usuarioDto = new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol.ToString(),
            TenantId = usuario.TenantId,
            TenantNombre = usuario.Tenant?.Nombre ?? string.Empty
        };

        return Ok(usuarioDto);
    }
}
