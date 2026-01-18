using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace QueueLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoisonController : ControllerBase
{
    private readonly ConnectionFactory _factory;
    private readonly ILogger<PoisonController> _logger;

    public PoisonController(ILogger<PoisonController> logger, IConfiguration configuration)
    {
        _factory = new ConnectionFactory
        {
            Uri = new Uri(configuration.GetConnectionString("RabbitMq") ?? "amqp://guest:guest@localhost:5672")
        };
        _logger = logger;
    }

    [HttpPost("publish")]
    public IActionResult Publish([FromQuery] string message, [FromQuery] int count = 1)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "poison-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        for(int i=0; i<count; i++)
        {
            channel.BasicPublish(exchange: "", routingKey: "poison-queue", basicProperties: null, body: body);
        }

        return Ok(new { Sent = count, Message = message });
    }
}
