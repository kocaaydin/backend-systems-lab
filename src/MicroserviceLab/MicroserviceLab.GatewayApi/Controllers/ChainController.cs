using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

namespace MicroserviceLab.GatewayApi.Controllers;

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
        using var activity = new ActivitySource("MicroserviceLab.GatewayApi").StartActivity("GatewayProcessing");
        
        _logger.LogInformation("Gateway received chain request. Forwarding to Order Service...");

        var orderServiceUrl = _config["OrderServiceUrl"] ?? "http://microservice-order-service:8080/api/orders"; 
        
        var client = _clientFactory.CreateClient();
        
        try 
        {
            var orderRequest = new { OrderId = Guid.NewGuid().ToString(), Amount = 99.99m };
            var response = await client.PostAsJsonAsync(orderServiceUrl, orderRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                activity?.SetTag("chain.status", "success");
                
                return Ok(new 
                { 
                    Message = "Chain Completed Successfully", 
                    OrderResult = result,
                    TraceId = Activity.Current?.TraceId.ToString()
                });
            }
            else
            {
                activity?.SetTag("chain.status", "failed");
                return StatusCode((int)response.StatusCode, "Order creation failed");
            }
        }
        catch (Exception ex)
        {
            activity?.SetTag("chain.status", "failed");
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            _logger.LogError(ex, "Chain failed at Gateway -> Order Service link");
            return Problem("Downstream service failed.");
        }
    }
}
