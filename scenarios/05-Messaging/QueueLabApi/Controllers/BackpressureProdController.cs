using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Confluent.Kafka;
using System.Text;

namespace QueueLabApi.Controllers;

[ApiController]
[Route("api/backpressure-prod")]
public class BackpressureProdController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public BackpressureProdController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("produce-rabbit")]
    public IActionResult ProduceRabbit([FromQuery] int count = 100)
    {
        var hostName = _configuration["RabbitMqHost"] ?? "localhost";
        var factory = new ConnectionFactory { HostName = hostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: "lab_backpressure_rabbit", durable: false, exclusive: false, autoDelete: false, arguments: null);

        for (int i = 0; i < count; i++)
        {
            var body = Encoding.UTF8.GetBytes($"Job-{i}");
            channel.BasicPublish(exchange: "", routingKey: "lab_backpressure_rabbit", basicProperties: null, body: body);
        }

        return Ok(new { status = "Sent to RabbitMQ", count });
    }

    [HttpPost("produce-kafka")]
    public async Task<IActionResult> ProduceKafka([FromQuery] int count = 100)
    {
        var bootstrapServers = _configuration["KafkaBootstrap"] ?? "localhost:9092";
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        
        // Using pattern for producer
        using var producer = new ProducerBuilder<Null, string>(config).Build();

        for (int i = 0; i < count; i++)
        {
            await producer.ProduceAsync("lab_backpressure_kafka", new Message<Null, string> { Value = $"Job-{i}" });
        }
        
        // Ensure all messages are sent
        producer.Flush(TimeSpan.FromSeconds(5));

        return Ok(new { status = "Sent to Kafka", count });
    }
}