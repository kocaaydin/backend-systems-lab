using Microsoft.AspNetCore.Mvc;

namespace BackendLab.Api.Controllers;

[ApiController]
[Route("experiments")]
public class ExperimentsController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<ExperimentsController> _logger;

    public ExperimentsController(IHttpClientFactory clientFactory, IConfiguration config, ILogger<ExperimentsController> logger)
    {
        _clientFactory = clientFactory;
        _config = config;
        _logger = logger;
    }

    [HttpGet("latency")]
    public async Task<IActionResult> Latency()
    {
        await Task.Delay(200);
        return Ok(new { Service = "BackendLab.Api", Status = "Online", Version = "1.0.0" });
    }

    [HttpGet("cpu")]
    public IActionResult Cpu([FromQuery] int? n)
    {
        var count = n ?? 10000;
        int primes = 0;
        for (int i = 2; i < count; i++)
        {
            bool isPrime = true;
            for (int j = 2; j <= Math.Sqrt(i); j++)
            {
                if (i % j == 0) 
                {
                    isPrime = false;
                    break;
                }
            }
            if (isPrime) primes++;
        }
        return Ok(new { PrimesFound = primes, TestedUpTo = count });
    }

    [HttpGet("http-limit")]
    public async Task<IActionResult> HttpLimit([FromQuery] int? limit)
    {
        var maxConn = limit ?? 10;
        var externalUrl = _config["ExternalServiceUrl"] ?? "http://localhost:8081";

        using var handler = new SocketsHttpHandler
        {
            MaxConnectionsPerServer = maxConn,
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        using var client = new HttpClient(handler);
        var sw = System.Diagnostics.Stopwatch.StartNew();
        
        try 
        {
            var response = await client.GetStringAsync(externalUrl);
            sw.Stop();
            return Ok(new { 
                Status = "Success", 
                LatencyMs = sw.ElapsedMilliseconds, 
                UsedLimit = maxConn,
                ExternalData = response.Trim()
            });
        }
        catch (Exception ex)
        {
            sw.Stop();
            return Problem($"External Call Failed: {ex.Message} (Latency: {sw.ElapsedMilliseconds}ms)");
        }
    }

    [HttpGet("rate-limit")]
    public async Task<IActionResult> RateLimit()
    {
        var externalUrl = _config["ExternalServiceUrl"] ?? "http://localhost:8081";
        var client = _clientFactory.CreateClient("RateLimitedClient");
        
        try 
        {
            var response = await client.GetAsync(externalUrl);
            
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                return StatusCode(429);
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(new { Status = "Allowed & Forwarded", ExternalData = content.Trim() });
        }
        catch (Exception ex)
        {
            return Problem($"External Call Failed: {ex.Message}");
        }
    }

    [HttpGet("bad-http-client")]
    public async Task<IActionResult> BadHttpClient()
    {
        var externalUrl = _config["ExternalServiceUrl"] ?? "http://localhost:8081";
        
        using var client = new HttpClient(); 
        
        try 
        {
            var response = await client.GetStringAsync(externalUrl);
            return Ok(new { Status = "Success", Length = response.Length });
        }
        catch (Exception ex)
        {
            return Problem($"Failed: {ex.Message}");
        }
    }
}
