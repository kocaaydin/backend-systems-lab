
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

namespace ObservabilityLab.GatewayApi.Controllers;

[ApiController]
[Route("experiments/microservice/chain")]
public class ChainController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<ChainController> _logger;

    public ChainController(IHttpClientFactory clientFactory, IConfiguration config, ILogger<ChainController> logger)
    {
        _clientFactory = clientFactory;
        _config = config;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> StartChain()
    {
        // 1. Gateway receives request (Activity started auto by ASP.NET Core)
        using var activity = new ActivitySource("ObservabilityLab.GatewayApi").StartActivity("GatewayProcessing");
        
        _logger.LogInformation("Gateway received chain request. Forwarding to Core Service...");

        // 2. Call Core Service (Simulated by calling our existing BackendLab.Api or StorageLab.OrderApi)
        // Let's assume StorageLab.OrderApi is acting as the "Downstream" for this demo
        var downstreamUrl = _config["DownstreamServiceUrl"] ?? "http://storage-order-api:8080/health"; 
        
        var client = _clientFactory.CreateClient();
        
        try 
        {
            var response = await client.GetStringAsync(downstreamUrl);
            activity?.SetTag("chain.status", "success");
            
            return Ok(new 
            { 
                Message = "Chain Completed Successfully", 
                DownstreamResponse = response,
                TraceId = Activity.Current?.TraceId.ToString()
            });
        }
        catch (Exception ex)
        {
            activity?.SetTag("chain.status", "failed");
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            _logger.LogError(ex, "Chain failed at Gateway -> Downstream link");
            return Problem("Downstream service failed.");
        }
    }
}
