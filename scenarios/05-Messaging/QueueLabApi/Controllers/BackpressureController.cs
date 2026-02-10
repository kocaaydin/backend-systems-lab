using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/backpressure")]
public sealed class BackpressureController : ControllerBase
{
    [HttpPost("produce")]
    public IActionResult Produce([FromServices] QueueLabState state, [FromQuery] int count)
    {
        var safe = Math.Clamp(count, 1, 1000);
        for (var i = 0; i < safe; i++)
        {
            state.BackpressureQueue.Enqueue($"bp-{state.NextId()}");
        }

        return Ok(new
        {
            accepted = safe,
            queueSize = state.BackpressureQueueCount,
            workerRatePerSec = 10
        });
    }
}
