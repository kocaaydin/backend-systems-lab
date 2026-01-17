using Microsoft.AspNetCore.Mvc;
using NetworkLab.Api.Services;
using System.Net.NetworkInformation;

namespace NetworkLab.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestResultController : ControllerBase
{
    private readonly ILogger<TestResultController> _logger;

    public TestResultController(ILogger<TestResultController> logger)
    {
        _logger = logger;
    }

    [HttpPost("run-bad-scenario")]
    public async Task<IActionResult> RunBadScenario([FromQuery] int requestCount = 100)
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        
        // Baseline measurement
        var baselineEphemeral = ipGlobalProperties.GetActiveTcpConnections()
            .Count(c => c.LocalEndPoint.Port >= 49152);
        var baselineTimeWait = ipGlobalProperties.GetActiveTcpConnections()
            .Count(c => c.State == System.Net.NetworkInformation.TcpState.TimeWait);

        _logger.LogInformation("Starting Bad HttpClient test with {Count} requests", requestCount);

        // Execute bad requests
        var startTime = DateTime.UtcNow;
        var successCount = 0;
        var failCount = 0;

        for (int i = 0; i < requestCount; i++)
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var response = await client.GetAsync("http://google.com");
                if (response.IsSuccessStatusCode) successCount++;
                else failCount++;
            }
            catch
            {
                failCount++;
            }
        }

        var duration = (DateTime.UtcNow - startTime).TotalSeconds;

        // After measurement
        await Task.Delay(1000); // Wait for connections to settle
        var afterEphemeral = ipGlobalProperties.GetActiveTcpConnections()
            .Count(c => c.LocalEndPoint.Port >= 49152);
        var afterTimeWait = ipGlobalProperties.GetActiveTcpConnections()
            .Count(c => c.State == System.Net.NetworkInformation.TcpState.TimeWait);

        var metrics = new
        {
            RequestCount = requestCount,
            SuccessCount = successCount,
            FailCount = failCount,
            DurationSeconds = duration,
            RequestsPerSecond = requestCount / duration,
            Baseline = new { EphemeralPorts = baselineEphemeral, TimeWait = baselineTimeWait },
            After = new { EphemeralPorts = afterEphemeral, TimeWait = afterTimeWait },
            Delta = new 
            { 
                EphemeralPorts = afterEphemeral - baselineEphemeral,
                TimeWait = afterTimeWait - baselineTimeWait 
            }
        };

        var observations = $"Bad HttpClient usage created {afterEphemeral - baselineEphemeral} new ephemeral ports and {afterTimeWait - baselineTimeWait} TIME_WAIT connections. Each request opened a new socket.";

        await ResultLogger.LogResultAsync("Bad HttpClient Usage", metrics, observations);

        return Ok(new
        {
            Scenario = "Bad HttpClient Usage",
            Metrics = metrics,
            Observations = observations,
            ResultSavedTo = "/app/results/network_lab_results.json"
        });
    }

    [HttpPost("run-good-scenario")]
    public async Task<IActionResult> RunGoodScenario([FromQuery] int requestCount = 100)
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        
        // Baseline
        var baselineEphemeral = ipGlobalProperties.GetActiveTcpConnections()
            .Count(c => c.LocalEndPoint.Port >= 49152);
        var baselineTimeWait = ipGlobalProperties.GetActiveTcpConnections()
            .Count(c => c.State == System.Net.NetworkInformation.TcpState.TimeWait);

        _logger.LogInformation("Starting Good HttpClient test with {Count} requests", requestCount);

        // Execute good requests (reuse HttpClient)
        var startTime = DateTime.UtcNow;
        var successCount = 0;
        var failCount = 0;

        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(5);

        for (int i = 0; i < requestCount; i++)
        {
            try
            {
                var response = await client.GetAsync("http://google.com");
                if (response.IsSuccessStatusCode) successCount++;
                else failCount++;
            }
            catch
            {
                failCount++;
            }
        }

        var duration = (DateTime.UtcNow - startTime).TotalSeconds;

        // After measurement
        await Task.Delay(1000);
        var afterEphemeral = ipGlobalProperties.GetActiveTcpConnections()
            .Count(c => c.LocalEndPoint.Port >= 49152);
        var afterTimeWait = ipGlobalProperties.GetActiveTcpConnections()
            .Count(c => c.State == System.Net.NetworkInformation.TcpState.TimeWait);

        var metrics = new
        {
            RequestCount = requestCount,
            SuccessCount = successCount,
            FailCount = failCount,
            DurationSeconds = duration,
            RequestsPerSecond = requestCount / duration,
            Baseline = new { EphemeralPorts = baselineEphemeral, TimeWait = baselineTimeWait },
            After = new { EphemeralPorts = afterEphemeral, TimeWait = afterTimeWait },
            Delta = new 
            { 
                EphemeralPorts = afterEphemeral - baselineEphemeral,
                TimeWait = afterTimeWait - baselineTimeWait 
            }
        };

        var observations = $"Good HttpClient usage (singleton) reused connections. Only {afterEphemeral - baselineEphemeral} new ephemeral ports created for {requestCount} requests. Connection pooling working efficiently.";

        await ResultLogger.LogResultAsync("Good HttpClient Usage", metrics, observations);

        return Ok(new
        {
            Scenario = "Good HttpClient Usage",
            Metrics = metrics,
            Observations = observations,
            ResultSavedTo = "/app/results/network_lab_results.json"
        });
    }

    [HttpGet("results")]
    public async Task<IActionResult> GetResults()
    {
        var filePath = "/app/results/network_lab_results.json";
        
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(new { Message = "No test results found. Run a test scenario first." });
        }

        var json = await System.IO.File.ReadAllTextAsync(filePath);
        return Content(json, "application/json");
    }
}
