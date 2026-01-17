using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;

namespace MicroserviceLab.GatewayApi.Controllers;

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
        var activity = Activity.Current;
        
        var connectionString = _config["ConnectionStrings:RabbitMq"] ?? "amqp://guest:guest@rabbitmq:5672";
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: "microservice-lab-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var props = channel.CreateBasicProperties();
        props.Headers = new Dictionary<string, object>();
        
        if (activity != null)
        {
            props.Headers["traceparent"] = activity.Id;
        }

        var body = Encoding.UTF8.GetBytes(message);

        _logger.LogInformation("Publishing message to RabbitMQ: {Message}", message);
        channel.BasicPublish(exchange: "", routingKey: "microservice-lab-queue", basicProperties: props, body: body);

        return Ok(new { Status = "Published", Message = message, TraceId = activity?.TraceId.ToString() });
    }
}
