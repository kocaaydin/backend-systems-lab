using Microsoft.AspNetCore.Mvc;

namespace K6TargetApi.Controllers;

[ApiController]
[Route("")]
public sealed class K6TargetController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "ok" });
    }

    [HttpGet("work")]
    public async Task<IActionResult> Work([FromQuery] int delayMs = 50, CancellationToken ct = default)
    {
        if (delayMs < 0 || delayMs > 5000)
        {
            return BadRequest("delayMs 0-5000 araliginda olmali");
        }

        await Task.Delay(delayMs, ct);
        return Ok(new { completed = true, delayMs });
    }
}
