
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;
using System.Diagnostics;

namespace ResilienceLab.Api.Controllers;

[ApiController]
[Route("experiments/resilience/retry")]
public class RetryController : ControllerBase
{
    private readonly ILogger<RetryController> _logger;
    private static int _failureCount = 0;
    private const int FailureThreshold = 3; // Fails 3 times, succeeds on 4th

    public RetryController(ILogger<RetryController> logger)
    {
        _logger = logger;
    }

    // --- TARGET ENDPOINT (Flaky) ---
    [HttpGet("flaky-resource")]
    public IActionResult FlakyResource()
    {
        _failureCount++;
        if (_failureCount <= FailureThreshold)
        {
            _logger.LogWarning("Flaky Resource Failed! Attempt: {Count}", _failureCount);
            return StatusCode(500, "Transient Error Occurred");
        }
        
        _failureCount = 0; // Reset for next demo
        return Ok("Success!");
    }

    // --- SCENARIO 1: NAIVE RETRY (Bad) ---
    [HttpGet("naive")]
    public async Task<IActionResult> NaiveRetry()
    {
        _failureCount = 0; // Reset state
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080") }; // Loopback or service name

        // Hardcoded loop - no backoff, no jitter
        for (int i = 0; i < 5; i++)
        {
            var response = await SimulateCallAsync();
            if (response.IsSuccessStatusCode) return Ok("Naive Retry Succeeded");
            _logger.LogWarning("Naive Retry Failed. Retrying immediately...");
        }

        return StatusCode(500, "Naive Retry Exhausted");
    }

    // --- SCENARIO 2: SMART RETRY (Polly) ---
    [HttpGet("smart")]
    public async Task<IActionResult> SmartRetry()
    {
        _failureCount = 0; // Reset state

        // Define Policy: Retry 5 times with Exponential Backoff
        var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 5,
                Delay = TimeSpan.FromMilliseconds(200),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    _logger.LogInformation("Polly Retry: Attempt {Attempt}, Waiting {Delay}ms", args.AttemptNumber, args.RetryDelay.TotalMilliseconds);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        var response = await pipeline.ExecuteAsync(async token => await SimulateCallAsync());

        if (response.IsSuccessStatusCode) return Ok("Smart Retry Succeeded");
        return StatusCode(500, "Smart Retry Exhausted");
    }

    private async Task<HttpResponseMessage> SimulateCallAsync()
    {
        // Internal loopback call to our own Flaky Resource
        // In real docker env, we should call own URL, but for simplicity we simulate the logic directly or call via HTTP
        // Calling via HTTP allows observing network trace.
        // Assuming we are running inside container, 'localhost' might not work if we bind to specific IP.
        // Let's mimic the failure logic DIRECTLY to avoid network complexity for this specific unit demo, 
        // OR better, make this method actually fail based on the static counter simulated above.
        
        await Task.Delay(50); // Network latency
        
        _failureCount++;
         if (_failureCount <= FailureThreshold)
        {
             return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        }
        return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
    }
}
