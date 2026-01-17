using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MicroserviceLab.OrderService.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IHttpClientFactory clientFactory, ILogger<OrderController> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        using var activity = new ActivitySource("MicroserviceLab.OrderService").StartActivity("CreateOrder");
        
        activity?.SetTag("order.id", request.OrderId);
        activity?.SetTag("order.amount", request.Amount);
        
        _logger.LogInformation("Order Service: Creating order {OrderId} for ${Amount}", request.OrderId, request.Amount);

        // Simulate order validation
        await Task.Delay(50);

        // Call Payment Service
        var client = _clientFactory.CreateClient();
        var paymentUrl = Environment.GetEnvironmentVariable("PAYMENT_SERVICE_URL") ?? "http://microservice-payment-service:8080/api/payments";
        
        try
        {
            var paymentRequest = new { OrderId = request.OrderId, Amount = request.Amount };
            var response = await client.PostAsJsonAsync(paymentUrl, paymentRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var paymentResult = await response.Content.ReadAsStringAsync();
                activity?.SetTag("order.status", "completed");
                
                return Ok(new 
                { 
                    OrderId = request.OrderId, 
                    Status = "Completed", 
                    Payment = paymentResult,
                    TraceId = Activity.Current?.TraceId.ToString()
                });
            }
            else
            {
                activity?.SetTag("order.status", "payment_failed");
                return StatusCode(500, "Payment failed");
            }
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            _logger.LogError(ex, "Failed to process payment for order {OrderId}", request.OrderId);
            return StatusCode(500, "Order processing failed");
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { Service = "OrderService", Status = "Healthy" });
    }
}

public record CreateOrderRequest(string OrderId, decimal Amount);
