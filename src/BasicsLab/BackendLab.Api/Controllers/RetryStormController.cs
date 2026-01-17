using Microsoft.AspNetCore.Mvc;

namespace BackendLab.Api.Controllers;

[ApiController]
[Route("experiments/resiliency/retry")]
public class RetryStormController : ControllerBase
{
    private static int _requestCount = 0;

    [HttpGet("target")]
    public IActionResult Target([FromQuery] double failureRate = 0.5)
    {
        _requestCount++;
        // Simulate a service that is struggling
        if (Random.Shared.NextDouble() < failureRate)
        {
            return StatusCode(503, "Service Unavailable (Simulated)");
        }
        return Ok($"Success! Total Requests Received: {_requestCount}");
    }
    
    // In a real lab, you would have a "Client" Controller that calls this "Target"
    // with different Retry Policies (None, Fixed, Exponential) to show the difference.
    // For now, valid k6 scripts hitting this endpoint directly with high RPS is enough to simulate the "Storm".
}
