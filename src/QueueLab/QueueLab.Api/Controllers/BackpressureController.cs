using Microsoft.AspNetCore.Mvc;
using QueueLab.Api.Services;

namespace QueueLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BackpressureController : ControllerBase
{
    private readonly BackpressureChannel _channel;

    public BackpressureController(BackpressureChannel channel)
    {
        _channel = channel;
    }

    [HttpPost("produce")]
    public async Task<IActionResult> Produce([FromQuery] int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            await _channel.Writer.WriteAsync(i);
        }
        return Ok(new { Message = $"Produced {count} items", CurrentQueueSize = _channel.Count });
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new { QueueSize = _channel.Count });
    }
}
