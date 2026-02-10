using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/metrics")]
public sealed class MetricsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromServices] QueueLabState state)
    {
        return Ok(new
        {
            state.BackpressureQueueCount,
            state.PoisonQueueCount,
            state.KafkaBacklogCount,
            state.RabbitInMemoryBufferCount,
            state.DlqCount,
            state.ProcessedBackpressure,
            state.ProcessedPoison,
            state.PoisonFailures,
            state.RebalancePauses
        });
    }
}
