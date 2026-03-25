using Confluent.Kafka;

namespace QueueLabApi.Workers;

public sealed class KafkaWorker : BackgroundService
{
    private readonly ILogger<KafkaWorker> _logger;
    private readonly IConfiguration _configuration;
    private const string Topic = "lab_backpressure_kafka";

    public KafkaWorker(ILogger<KafkaWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServers = _configuration["KafkaBootstrap"] ?? "localhost:9092";
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "lab-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false // Backpressure kontrolü için manuel commit
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
                consumer.Subscribe(Topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        // Kafka'dan çek (Pull Model)
                        var cr = consumer.Consume(stoppingToken);

                        // Yavaş işlem simülasyonu
                        await Task.Delay(100, stoppingToken);

                        // İşlendi, commit et
                        consumer.Commit(cr);
                    }
                    catch (ConsumeException) { /* Ignore transient */ }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Kafka waiting... ({Message})", ex.Message);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}