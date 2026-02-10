using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/hol")]
public sealed class HolController : ControllerBase
{
    [HttpPost("job")]
    public async Task<IActionResult> Enqueue([FromServices] QueueLabState state, [FromQuery] int durationMs, CancellationToken ct)
    {
        var safe = Math.Clamp(durationMs, 1, 10000);
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        state.HolQueue.Enqueue(new HolJob(safe, tcs));

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(15));

        try
        {
            await tcs.Task.WaitAsync(timeoutCts.Token);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(504);
        }

        return Ok(new
        {
            accepted = true,
            durationMs = safe,
            holQueueSize = state.HolQueueCount
        });
    }
}
