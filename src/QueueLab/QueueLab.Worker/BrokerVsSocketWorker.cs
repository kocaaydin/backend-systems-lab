using Confluent.Kafka;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueLab.Worker;

// Scenarios 7 & 8: Broker Backlog vs Socket Backlog & Propagation
public class BrokerVsSocketWorker : BackgroundService
{
    private readonly ILogger<BrokerVsSocketWorker> _logger;
    private readonly IConfiguration _configuration;

    public BrokerVsSocketWorker(ILogger<BrokerVsSocketWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Run both comparisons in parallel
        var kafkaTask = RunKafkaSlowConsumer(stoppingToken);
        var rabbitTask = RunRabbitMqPrefetchConsumer(stoppingToken);

        await Task.WhenAll(kafkaTask, rabbitTask);
    }

    private async Task RunKafkaSlowConsumer(CancellationToken stoppingToken)
    {
        // KAFKA: PULL Model.
        // If we process slowly, we just request offsets less frequently.
        // Messages pile up on BROKER DISK.
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration.GetConnectionString("Kafka") ?? "localhost:9092",
            GroupId = "slow-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true 
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("kafka-slow-topic");

        _logger.LogInformation("[KafkaSlowConsumer] Started. I will poll very slowly.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try 
            {
                var result = consumer.Consume(TimeSpan.FromSeconds(1));
                if (result != null)
                {
                    _logger.LogInformation($"[KafkaSlowConsumer] Pulled msg: {result.Message.Value}. Now sleeping 2s...");
                    // This sleep ensures we don't ask the broker for more. 
                    // Network traffic is low. Broker retains messages.
                    await Task.Delay(2000, stoppingToken);
                }
            }
            catch (Exception) { /* Ignored */ }
        }
    }

    private Task RunRabbitMqPrefetchConsumer(CancellationToken stoppingToken)
    {
        // RABBITMQ: PUSH Model (mostly).
        // If Prefetch is HIGH, broker pushes messages to our TCP Socket -> Client Buffer.
        // Messages act as if they are "Dequeued" from Broker RAM (marked as Delivered but Unacked),
        // but they sit in Client RAM.
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration.GetConnectionString("RabbitMq") ?? "amqp://guest:guest@localhost:5672"),
            DispatchConsumersAsync = true
        };

        try
        {
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "rabbit-socket-pressure-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            
            // High Pretech: Give me 1000 messages at once!
            channel.BasicQos(0, 1000, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                // We received it (it's in our buffer/memory now), but we process slow.
                // The broker has already sent it over the wire.
                _logger.LogInformation($"[RabbitSocketPressure] Received. Sleeping 2s...");
                await Task.Delay(2000, stoppingToken);
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume("rabbit-socket-pressure-queue", false, consumer);
            _logger.LogInformation("[RabbitSocketPressure] Started with Prefetch=1000. Messages will pile up in Socket/Client RAM.");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ Connect Fail");
        }

        return Task.CompletedTask;
    }
}
