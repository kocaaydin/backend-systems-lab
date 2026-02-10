using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/poison")]
public sealed class PoisonController : ControllerBase
{
    [HttpPost("publish")]
    public IActionResult Publish([FromServices] QueueLabState state, [FromQuery] string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return BadRequest(new { error = "message zorunlu" });
        }

        state.PoisonQueue.Enqueue(message);
        return Ok(new
        {
            accepted = true,
            message,
            poisonQueueSize = state.PoisonQueueCount
        });
    }
}
