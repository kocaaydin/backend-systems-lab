
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;

namespace ObservabilityLab.GatewayApi.Controllers;

[ApiController]
[Route("experiments/microservice/queue")]
public class QueueController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<QueueController> _logger;

    public QueueController(IConfiguration config, ILogger<QueueController> logger)
    {
        _config = config;
        _logger = logger;
    }

    [HttpPost("publish")]
    public IActionResult PublishMessage([FromQuery] string message = "Hello from Gateway")
    {
        var activity = Activity.Current; // It's already started by ASP.NET Core
        
        // --- 1. Connect to RabbitMQ ---
        var connectionString = _config["ConnectionStrings:RabbitMq"] ?? "amqp://guest:guest@rabbitmq:5672";
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: "microservice-lab-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        // --- 2. Inject Trace Context into Headers ---
        // Vital for Async Tracing! We must manually pass TraceId.
        var props = channel.CreateBasicProperties();
        props.Headers = new Dictionary<string, object>();
        
        if (activity != null)
        {
            // Simple W3C Propagation manually just for demo
            props.Headers["traceparent"] = activity.Id;
            // Or better, use OTel Propagator (but manual is educational here)
        }

        var body = Encoding.UTF8.GetBytes(message);

        // --- 3. Publish ---
        _logger.LogInformation("Publishing message to RabbitMQ: {Message}", message);
        channel.BasicPublish(exchange: "", routingKey: "microservice-lab-queue", basicProperties: props, body: body);

        return Ok(new { Status = "Published", Message = message, TraceId = activity?.TraceId.ToString() });
    }
}
