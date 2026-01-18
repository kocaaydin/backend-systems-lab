using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using QueueLab.Api.Services;

namespace QueueLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HolController : ControllerBase
{
    private readonly HolChannel _channel;
    // Static dictionary to coordinate wait (simplification for lab demo)
    public static readonly ConcurrentDictionary<int, TaskCompletionSource<bool>> PendingJobs = new();
    private static int _idCounter = 0;

    public HolController(HolChannel channel)
    {
        _channel = channel;
    }

    [HttpPost("job")]
    public async Task<IActionResult> SubmitJob([FromQuery] int durationMs)
    {
        var id = Interlocked.Increment(ref _idCounter);
        var tcs = new TaskCompletionSource<bool>();
        PendingJobs.TryAdd(id, tcs);

        await _channel.Writer.WriteAsync(new HolJob(id, durationMs));

        // Wait for worker to complete this specific job
        // If queue is backed up by slow jobs, this wait will be long
        await tcs.Task;

        return Ok(new { Id = id, Duration = durationMs, Message = "Completed" });
    }
}
