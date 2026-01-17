using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace MicroserviceLab.GatewayApi.Controllers;

[ApiController]
[Route("experiments/microservice/kafka")]
public class KafkaController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<KafkaController> _logger;
    private readonly string _bootstrapServers;

    public KafkaController(IConfiguration config, ILogger<KafkaController> logger)
    {
        _config = config;
        _logger = logger;
        _bootstrapServers = _config["ConnectionStrings:Kafka"] ?? "kafka:29092";
    }

    [HttpPost("produce")]
    public async Task<IActionResult> ProduceMessage([FromQuery] string message = "Hello via Kafka")
    {
        var activity = Activity.Current;

        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        try 
        {
            var headers = new Headers();
            if (activity != null && activity.Id != null)
            {
                headers.Add("traceparent", Encoding.UTF8.GetBytes(activity.Id));
            }

            var dr = await producer.ProduceAsync("microservice-lab-topic", new Message<Null, string> { Value = message, Headers = headers });

            _logger.LogInformation("Delivered '{Message}' to '{TopicPartitionOffset}'", dr.Value, dr.TopicPartitionOffset);

            return Ok(new 
            { 
                Status = "Produced to Kafka", 
                Topic = "microservice-lab-topic", 
                Offset = dr.Offset.Value, 
                TraceId = activity?.TraceId.ToString() 
            });
        }
        catch (ProduceException<Null, string> e)
        {
             _logger.LogError(e, "Kafka delivery failed");
             return Problem($"Delivery failed: {e.Error.Reason}");
        }
    }
}
