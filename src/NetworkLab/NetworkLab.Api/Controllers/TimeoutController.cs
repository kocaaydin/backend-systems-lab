using Microsoft.AspNetCore.Mvc;

namespace NetworkLab.Api.Controllers;

[ApiController]
[Route("api/timeout")]
public class TimeoutController : ControllerBase
{
    private readonly ILogger<TimeoutController> _logger;

    public TimeoutController(ILogger<TimeoutController> logger)
    {
        _logger = logger;
    }

    [HttpGet("long-process")]
    public async Task<IActionResult> LongProcess([FromQuery] int durationSeconds = 10, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting long process for {Duration} seconds", durationSeconds);

        try
        {
            for (int i = 0; i < durationSeconds; i++)
            {
                // Check if client cancelled
                cancellationToken.ThrowIfCancellationRequested();
                
                await Task.Delay(1000, cancellationToken);
                _logger.LogInformation("Processing... {Progress}/{Total}", i + 1, durationSeconds);
            }

            _logger.LogInformation("Long process completed successfully");
            return Ok(new
            {
                Status = "Completed",
                DurationSeconds = durationSeconds,
                Message = "Server finished processing despite potential client timeout"
            });
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Long process was cancelled by client");
            return StatusCode(499, new
            {
                Status = "Cancelled",
                Message = "Client disconnected, server stopped processing (CancellationToken worked!)"
            });
        }
    }

    [HttpGet("long-process-no-cancellation")]
    public async Task<IActionResult> LongProcessNoCancellation([FromQuery] int durationSeconds = 10)
    {
        _logger.LogInformation("Starting long process WITHOUT cancellation token for {Duration} seconds", durationSeconds);

        for (int i = 0; i < durationSeconds; i++)
        {
            await Task.Delay(1000);
            _logger.LogInformation("Processing (no cancellation check)... {Progress}/{Total}", i + 1, durationSeconds);
        }

        _logger.LogInformation("Long process completed (even if client timed out)");
        return Ok(new
        {
            Status = "Completed",
            DurationSeconds = durationSeconds,
            Message = "Server finished processing even if client timed out (NO CancellationToken!)"
        });
    }
}
