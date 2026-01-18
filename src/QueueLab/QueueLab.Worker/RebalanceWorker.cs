using Confluent.Kafka;

namespace QueueLab.Worker;

public class RebalanceWorker : BackgroundService
{
    private readonly ILogger<RebalanceWorker> _logger;
    private readonly IConfiguration _configuration;

    public RebalanceWorker(ILogger<RebalanceWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration.GetConnectionString("Kafka") ?? "localhost:9092",
            GroupId = "rebalance-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            SessionTimeoutMs = 6000,
            HeartbeatIntervalMs = 2000
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config)
            .SetPartitionsAssignedHandler((c, partitions) =>
            {
                _logger.LogWarning($"[Rebalance] Partitions Assigned: {string.Join(", ", partitions.Select(p => p.Partition.Value))}");
            })
            .SetPartitionsRevokedHandler((c, partitions) =>
            {
                _logger.LogWarning($"[Rebalance] Partitions Revoked (Stop the world): {string.Join(", ", partitions.Select(p => p.Partition.Value))}");
            })
            .Build();

        consumer.Subscribe("rebalance-topic");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                   // _logger.LogInformation($"[Rebalance] Processing: {result.Message.Value}");
                   // Keep logs quiet to highlight rebalance events
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kafka Consume Error");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fatal Rebalance Worker Error");
        }
    }
}
