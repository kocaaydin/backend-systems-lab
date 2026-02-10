using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tcplab")]
public sealed class TcpLabController : ControllerBase
{
    [HttpPost("flood-saturation")]
    public IActionResult FloodSaturation([FromServices] QueueLabState state, [FromQuery] int count)
    {
        var safe = Math.Clamp(count, 1, 20000);
        for (var i = 0; i < safe; i++)
        {
            state.RabbitInMemoryBuffer.Enqueue(new string('x', 256));
        }

        return Ok(new
        {
            accepted = safe,
            inMemoryBuffer = state.RabbitInMemoryBufferCount,
            estimatedBytes = state.RabbitInMemoryBufferCount * 256
        });
    }

    [HttpPost("fill-kafka-slow")]
    public IActionResult FillKafkaSlow([FromServices] QueueLabState state, [FromQuery] int count)
    {
        var safe = Math.Clamp(count, 1, 5000);
        for (var i = 0; i < safe; i++)
        {
            state.KafkaBacklog.Enqueue($"kafka-{state.NextId()}");
        }

        return Ok(new
        {
            accepted = safe,
            kafkaBacklog = state.KafkaBacklogCount
        });
    }

    [HttpPost("fill-rabbit-pressure")]
    public IActionResult FillRabbitPressure([FromServices] QueueLabState state, [FromQuery] int count)
    {
        var safe = Math.Clamp(count, 1, 5000);
        for (var i = 0; i < safe; i++)
        {
            state.RabbitInMemoryBuffer.Enqueue(new string('r', 128));
        }

        return Ok(new
        {
            accepted = safe,
            rabbitBuffer = state.RabbitInMemoryBufferCount,
            estimatedBytes = state.RabbitInMemoryBufferCount * 128
        });
    }

    [HttpPost("churn-load")]
    public IActionResult ChurnLoad([FromServices] QueueLabState state, [FromQuery] int count)
    {
        var safe = Math.Clamp(count, 1, 5000);
        for (var i = 0; i < safe; i++)
        {
            state.KafkaBacklog.Enqueue($"churn-{state.NextId()}");
        }

        state.ConnectionChurnSignals.Enqueue(1);
        return Ok(new
        {
            accepted = safe,
            churnSignals = state.ConnectionChurnSignals.Count,
            kafkaBacklog = state.KafkaBacklogCount
        });
    }
}
