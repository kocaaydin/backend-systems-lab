using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/burst")]
public sealed class BurstController : ControllerBase
{
    [HttpGet("work")]
    public async Task<IActionResult> Work([FromQuery] int intensity, CancellationToken ct)
    {
        var safe = Math.Clamp(intensity, 1, 500);
        await Task.Delay(safe, ct);

        return Ok(new
        {
            accepted = true,
            intensity = safe
        });
    }
}
