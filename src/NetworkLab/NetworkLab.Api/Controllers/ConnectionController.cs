
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NetworkLab.Api.Controllers;

[ApiController]
[Route("experiments/network/connection")]
public class ConnectionController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ConnectionController> _logger;

    public ConnectionController(IHttpClientFactory clientFactory, ILogger<ConnectionController> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    // --- SCENARIO 1: BAD USAGE (Socket Exhaustion Risk) ---
    [HttpGet("bad")]
    public async Task<IActionResult> BadUsage()
    {
        // ANTI-PATTERN: Creating new HttpClient for every request
        // This leaves sockets in TIME_WAIT state
        using var client = new HttpClient(); 
        
        try 
        {
            var response = await client.GetAsync("http://external-api:80"); // Local service
            return Ok($"Status: {response.StatusCode} (New Socket Used)");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    // --- SCENARIO 2: GOOD USAGE (Connection Reuse) ---
    [HttpGet("good")]
    public async Task<IActionResult> GoodUsage()
    {
        // PATTERN: Using Factory (Reuse underlying handler)
        var client = _clientFactory.CreateClient();
        
        try 
        {
            var response = await client.GetAsync("http://external-api:80");
            return Ok($"Status: {response.StatusCode} (Socket Reused)");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
