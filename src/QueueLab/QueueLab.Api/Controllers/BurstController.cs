using Microsoft.AspNetCore.Mvc;

namespace QueueLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BurstController : ControllerBase
{
    [HttpGet("work")]
    public async Task<IActionResult> Work([FromQuery] int intensity = 10)
    {
        // Simulate CPU or IO load
        // "intensity" roughly maps to milliseconds of load
        await Task.Delay(intensity);
        return Ok(new { Worked = true });
    }
}
