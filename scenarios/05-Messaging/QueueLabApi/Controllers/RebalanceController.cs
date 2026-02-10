using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/rebalance")]
public sealed class RebalanceController : ControllerBase
{
    [HttpPost("produce")]
    public IActionResult Produce([FromServices] QueueLabState state, [FromQuery] int count)
    {
        var safe = Math.Clamp(count, 1, 1000);
        for (var i = 0; i < safe; i++)
        {
            state.KafkaBacklog.Enqueue($"rebalance-{state.NextId()}");
        }

        return Ok(new
        {
            accepted = safe,
            kafkaBacklog = state.KafkaBacklogCount,
            note = "Rebalance simulasyonu worker tarafinda donemsel pause ile yapilir"
        });
    }
}
