using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MicroserviceLab.PaymentService.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(ILogger<PaymentController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        using var activity = new ActivitySource("MicroserviceLab.PaymentService").StartActivity("ProcessPayment");
        
        activity?.SetTag("payment.order_id", request.OrderId);
        activity?.SetTag("payment.amount", request.Amount);
        
        _logger.LogInformation("Payment Service: Processing payment for order {OrderId}, amount ${Amount}", 
            request.OrderId, request.Amount);

        // Simulate payment processing
        await Task.Delay(100);

        // Simulate 10% payment failure rate
        var random = new Random();
        if (random.Next(100) < 10)
        {
            activity?.SetTag("payment.status", "failed");
            activity?.SetStatus(ActivityStatusCode.Error, "Insufficient funds");
            _logger.LogWarning("Payment failed for order {OrderId}", request.OrderId);
            return BadRequest(new { Status = "Failed", Reason = "Insufficient funds" });
        }

        activity?.SetTag("payment.status", "success");
        
        return Ok(new 
        { 
            Status = "Success", 
            TransactionId = Guid.NewGuid().ToString(),
            OrderId = request.OrderId,
            Amount = request.Amount,
            TraceId = Activity.Current?.TraceId.ToString()
        });
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { Service = "PaymentService", Status = "Healthy" });
    }
}

public record PaymentRequest(string OrderId, decimal Amount);
