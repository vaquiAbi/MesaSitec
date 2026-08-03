using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { estado = "ok" });
    }
}
