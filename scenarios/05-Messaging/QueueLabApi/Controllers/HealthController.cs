using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public sealed class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Get([FromServices] QueueLabState state)
    {
        return Ok(new
        {
            service = "queue-lab-api",
            status = "ok",
            state.BackpressureQueueCount,
            state.PoisonQueueCount,
            state.KafkaBacklogCount,
            state.RabbitInMemoryBufferCount,
            state.DlqCount
        });
    }
}
