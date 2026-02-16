using Microsoft.AspNetCore.Mvc;

namespace ThreadTypesApi.Controllers;

[ApiController]
[Route("")]
public sealed class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "ok", service = "thread-types-api" });
}
