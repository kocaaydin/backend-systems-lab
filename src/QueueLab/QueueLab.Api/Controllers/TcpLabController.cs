using System.Text;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace QueueLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TcpLabController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TcpLabController> _logger;

    public TcpLabController(ILogger<TcpLabController> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("flood-saturation")]
    public IActionResult FloodSaturation([FromQuery] int count = 10000)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration.GetConnectionString("RabbitMq") ?? "amqp://guest:guest@localhost:5672")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "saturation-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(new string('A', 1024)); // 1KB payload

        // Batch publish for speed
        var batch = channel.CreateBasicPublishBatch();
        for (int i = 0; i < count; i++)
        {
            batch.Add(exchange: "", routingKey: "saturation-queue", mandatory: false, properties: null, body: body);
        }
        batch.Publish();

        return Ok(new { Sent = count, Target = "saturation-queue" });
    }

    [HttpPost("fill-kafka-slow")]
    public async Task<IActionResult> FillKafkaSlow([FromQuery] int count = 100)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _configuration.GetConnectionString("Kafka") ?? "localhost:9092"
        };
        using var producer = new ProducerBuilder<Null, string>(config).Build();

        for (int i = 0; i < count; i++)
        {
            await producer.ProduceAsync("kafka-slow-topic", new Message<Null, string> { Value = $"SlowMsg-{i}" });
        }
        
        return Ok(new { Sent = count, Target = "kafka-slow-topic" });
    }

    [HttpPost("fill-rabbit-pressure")]
    public IActionResult FillRabbitPressure([FromQuery] int count = 100)
    {
         var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration.GetConnectionString("RabbitMq") ?? "amqp://guest:guest@localhost:5672")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "rabbit-socket-pressure-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        
        for(int i=0; i<count; i++)
        {
             channel.BasicPublish("", "rabbit-socket-pressure-queue", null, Encoding.UTF8.GetBytes($"Pressure-{i}"));
        }
        
        return Ok(new { Sent = count, Target = "rabbit-socket-pressure-queue" });
    }
    
    [HttpPost("churn-load")]
    public async Task<IActionResult> ChurnLoad([FromQuery] int count = 100)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _configuration.GetConnectionString("Kafka") ?? "localhost:9092"
        };
        using var producer = new ProducerBuilder<Null, string>(config).Build();

        for (int i = 0; i < count; i++)
        {
             // Just produce noise to keep the churn worker busy looking for data
            await producer.ProduceAsync("churn-topic", new Message<Null, string> { Value = $"Churn-{i}" });
        }
        
        return Ok(new { Sent = count, Target = "churn-topic" });
    }
}
