using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public sealed class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Get()
    {
        return Ok(new { status = "ok", service = "connection-pool-api" });
    }
}
