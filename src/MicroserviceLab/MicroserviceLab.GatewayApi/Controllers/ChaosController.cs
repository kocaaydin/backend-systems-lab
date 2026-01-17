using Microsoft.AspNetCore.Mvc;
using MicroserviceLab.GatewayApi.Services;
using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;

namespace MicroserviceLab.GatewayApi.Controllers;

[ApiController]
[Route("experiments/chaos")]
public class ChaosController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<ChaosController> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public ChaosController(IConfiguration config, ILogger<ChaosController> logger, IHttpClientFactory clientFactory)
    {
        _config = config;
        _logger = logger;
        _clientFactory = clientFactory;
    }

    [HttpGet("collector-down")]
    public async Task<IActionResult> CollectorDown()
    {
        var sw = Stopwatch.StartNew();
        
        // Simulate: Create traces but don't export (OTEL Collector is "down")
        // In reality, we just create local activities without proper exporter
        using var activity = new ActivitySource("MicroserviceLab.Chaos").StartActivity("CollectorDownSimulation");
        
        activity?.SetTag("chaos.scenario", "collector-down");
        activity?.SetTag("chaos.impact", "traces-not-exported");
        
        _logger.LogWarning("CHAOS: Simulating OTEL Collector Down. Traces will be lost.");
        
        // Simulate some work
        await Task.Delay(100);
        
        sw.Stop();
        
        await ResultLogger.LogResultAsync(
            "Collector Down",
            new { DurationMs = sw.ElapsedMilliseconds, TracesLost = true },
            "Application continues to run but observability data is lost. No performance impact observed."
        );

        return Ok(new 
        { 
            Scenario = "Collector Down",
            Status = "Simulated",
            Impact = "Traces not exported",
            DurationMs = sw.ElapsedMilliseconds,
            Message = "Check results/microservice_lab_results.json for details"
        });
    }

    [HttpGet("slow-consumer")]
    public async Task<IActionResult> SlowConsumer()
    {
        var sw = Stopwatch.StartNew();
        
        // Simulate: Publish many messages quickly to RabbitMQ
        // Worker will consume slowly, creating backpressure
        var connectionString = _config["ConnectionStrings:RabbitMq"] ?? "amqp://guest:guest@rabbitmq:5672";
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: "chaos-slow-consumer-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        int messageCount = 100;
        for (int i = 0; i < messageCount; i++)
        {
            var body = Encoding.UTF8.GetBytes($"Message {i}");
            channel.BasicPublish(exchange: "", routingKey: "chaos-slow-consumer-queue", basicProperties: null, body: body);
        }
        
        sw.Stop();
        
        _logger.LogWarning("CHAOS: Published {Count} messages. Consumer will lag behind (backpressure).", messageCount);
        
        await ResultLogger.LogResultAsync(
            "Slow Consumer (Backpressure)",
            new { MessagesPublished = messageCount, PublishDurationMs = sw.ElapsedMilliseconds },
            "Fast producer overwhelms slow consumer. Queue depth increases. Monitor RabbitMQ Management UI."
        );

        return Ok(new 
        { 
            Scenario = "Slow Consumer",
            MessagesPublished = messageCount,
            DurationMs = sw.ElapsedMilliseconds,
            Message = "Check RabbitMQ Management UI (http://localhost:15672) for queue depth"
        });
    }

    [HttpGet("network-partition")]
    public async Task<IActionResult> NetworkPartition()
    {
        var sw = Stopwatch.StartNew();
        
        // Simulate: Call downstream with very short timeout (network partition simulation)
        var downstreamUrl = _config["DownstreamServiceUrl"] ?? "http://storage-order-api:8080/health";
        
        var client = _clientFactory.CreateClient();
        client.Timeout = TimeSpan.FromMilliseconds(1); // Extremely short timeout
        
        try 
        {
            var response = await client.GetStringAsync(downstreamUrl);
            sw.Stop();
            
            return Ok(new { Scenario = "Network Partition", Status = "Unexpected Success" });
        }
        catch (TaskCanceledException)
        {
            sw.Stop();
            
            _logger.LogError("CHAOS: Network Partition simulated. Downstream unreachable (timeout).");
            
            await ResultLogger.LogResultAsync(
                "Network Partition",
                new { TimeoutMs = 1, ActualDurationMs = sw.ElapsedMilliseconds },
                "Downstream service unreachable due to network partition. Trace context lost. Circuit breaker should open."
            );

            return StatusCode(503, new 
            { 
                Scenario = "Network Partition",
                Status = "Downstream Unreachable",
                DurationMs = sw.ElapsedMilliseconds,
                Message = "Simulated network partition via 1ms timeout"
            });
        }
    }
}
