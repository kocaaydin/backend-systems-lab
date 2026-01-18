using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace QueueLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RebalanceController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RebalanceController> _logger;

    public RebalanceController(ILogger<RebalanceController> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("produce")]
    public async Task<IActionResult> Produce([FromQuery] int count = 1000)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _configuration.GetConnectionString("Kafka") ?? "localhost:9092"
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        for (int i = 0; i < count; i++)
        {
            await producer.ProduceAsync("rebalance-topic", new Message<Null, string> { Value = $"Msg-{i}" });
        }
        
        // Wait for delivery to make sure topic exists and has data
        producer.Flush(TimeSpan.FromSeconds(5));

        return Ok(new { Produced = count, Topic = "rebalance-topic" });
    }
}
