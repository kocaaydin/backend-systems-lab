
using Confluent.Kafka;
using System.Diagnostics;
using System.Text;

namespace BackendLab.Worker;

public class KafkaWorker : BackgroundService
{
    private readonly ILogger<KafkaWorker> _logger;
    private readonly string _bootstrapServers;
    private readonly string _topic = "microservice-lab-topic";

    public KafkaWorker(ILogger<KafkaWorker> logger, IConfiguration config)
    {
        _logger = logger;
        _bootstrapServers = config["ConnectionStrings:Kafka"] ?? "kafka:29092";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Kafka Worker Started - Listening on {Topic}", _topic);

        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "backend-lab-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try 
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    // EXTRACT TRACE CONTEXT
                    ActivityContext parentContext = default;
                    if (consumeResult.Message.Headers.TryGetLastBytes("traceparent", out var traceParentBytes))
                    {
                        var traceParent = Encoding.UTF8.GetString(traceParentBytes);
                        ActivityContext.TryParse(traceParent, null, out parentContext);
                    }

                    // START CONSUMER ACTIVITY
                    using var activity = new ActivitySource("BackendLab.Worker").StartActivity("ProcessKafkaMessage", ActivityKind.Consumer, parentContext);
                    
                    activity?.SetTag("messaging.system", "kafka");
                    activity?.SetTag("messaging.destination", _topic);

                    _logger.LogInformation("[Kafka] Received: {Value} | TraceId: {TraceId}", consumeResult.Message.Value, activity?.TraceId);

                    // Simulate processing
                    await Task.Delay(50, stoppingToken);
                }
                catch (ConsumeException e)
                {
                    _logger.LogError("Error consuming: {Reason}", e.Error.Reason);
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}
