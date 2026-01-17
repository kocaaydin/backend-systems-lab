
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ObservabilityLab.OrderApi.Controllers;

[ApiController]
[Route("experiments/observability")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder()
    {
        // 1. Check for Observability Mode Header
        var mode = Request.Headers["X-Observability-Mode"].FirstOrDefault() ?? "VendorFree";
        
        // This is a simulation! In real vendor-lock scenario, you would have code like:
        // if (mode == "VendorLocked") NewRelic.Api.Agent.NewRelic.RecordMetric(...)
        
        using var activity = new ActivitySource("ObservabilityLab.OrderApi").StartActivity("ProcessOrder");
        activity?.SetTag("observability.mode", mode);

        _logger.LogInformation("Processing Order in {Mode} mode", mode);

        if (mode == "VendorLocked")
        {
            // Simulate direct vendor call logic
            // In a real app, this would be `NewRelic.IgnoreTransaction()` or specific proprietary API usage
            activity?.SetTag("vendor", "new_relic_proprietary");
            await Task.Delay(100); 
            return Ok(new { OrderId = Guid.NewGuid(), Mode = "VendorLocked (Mock)" });
        }
        else
        {
            // Standard OTel Path
            activity?.SetTag("vendor", "open_standards");
            await Task.Delay(100);
            return Ok(new { OrderId = Guid.NewGuid(), Mode = "VendorFree (OTel)" });
        }
    }
}
