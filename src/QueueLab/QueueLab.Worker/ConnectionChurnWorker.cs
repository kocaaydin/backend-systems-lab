using Confluent.Kafka;

namespace QueueLab.Worker;

// Scenario 9: Connection Churn
public class ConnectionChurnWorker : BackgroundService
{
    private readonly ILogger<ConnectionChurnWorker> _logger;
    private readonly IConfiguration _configuration;

    public ConnectionChurnWorker(ILogger<ConnectionChurnWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration.GetConnectionString("Kafka") ?? "localhost:9092",
            GroupId = "churn-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _logger.LogInformation("[ConnectionChurn] Starting violent connect/disconnect loops...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe("churn-topic");
                    
                    // Consume just a little bit
                    var result = consumer.Consume(TimeSpan.FromMilliseconds(500));
                    if (result != null)
                    {
                        // _logger.LogInformation($"[Churn] Got msg");
                    }
                    
                    // INTENTIONALLY DISPOSE AND RECREATE
                    // This forces TCP Fin/Syn and Rebalance protocol overhead repeatedly
                }

                _logger.LogWarning("[Churn] Connection dropped. Reconnecting immediately...");
                // No sleep, or very short sleep to spam the broker
                await Task.Delay(100, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Churn Loop Error");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
