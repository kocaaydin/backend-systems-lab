using Microsoft.AspNetCore.Mvc;

namespace NetworkLab.Api.Controllers;

[ApiController]
[Route("api/protocol")]
public class ProtocolController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ProtocolController> _logger;

    public ProtocolController(IHttpClientFactory clientFactory, ILogger<ProtocolController> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    [HttpGet("http1-test")]
    public async Task<IActionResult> Http1Test([FromQuery] int parallelRequests = 10)
    {
        _logger.LogInformation("Testing HTTP/1.1 with {Count} parallel requests", parallelRequests);

        var client = _clientFactory.CreateClient();
        client.DefaultRequestVersion = new Version(1, 1);

        var startTime = DateTime.UtcNow;
        var tasks = new List<Task<HttpResponseMessage>>();

        for (int i = 0; i < parallelRequests; i++)
        {
            tasks.Add(client.GetAsync("http://google.com"));
        }

        await Task.WhenAll(tasks);
        var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

        var successCount = tasks.Count(t => t.Result.IsSuccessStatusCode);

        return Ok(new
        {
            Protocol = "HTTP/1.1",
            ParallelRequests = parallelRequests,
            SuccessCount = successCount,
            DurationMs = duration,
            AvgLatencyMs = duration / parallelRequests,
            Note = "HTTP/1.1 uses multiple TCP connections (6-10 per host)"
        });
    }

    [HttpGet("http2-test")]
    public async Task<IActionResult> Http2Test([FromQuery] int parallelRequests = 10)
    {
        _logger.LogInformation("Testing HTTP/2 with {Count} parallel requests", parallelRequests);

        var client = _clientFactory.CreateClient();
        client.DefaultRequestVersion = new Version(2, 0);

        var startTime = DateTime.UtcNow;
        var tasks = new List<Task<HttpResponseMessage>>();

        for (int i = 0; i < parallelRequests; i++)
        {
            tasks.Add(client.GetAsync("https://www.google.com")); // HTTPS required for HTTP/2
        }

        await Task.WhenAll(tasks);
        var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

        var successCount = tasks.Count(t => t.Result.IsSuccessStatusCode);

        return Ok(new
        {
            Protocol = "HTTP/2",
            ParallelRequests = parallelRequests,
            SuccessCount = successCount,
            DurationMs = duration,
            AvgLatencyMs = duration / parallelRequests,
            Note = "HTTP/2 uses single TCP connection with multiplexing"
        });
    }
}
