using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueLab.Worker;

// Scenarios 6 & 10: TCP Buffer Saturation & App Memory vs Network Boundary
public class BufferSaturationWorker : BackgroundService
{
    private readonly ILogger<BufferSaturationWorker> _logger;
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;
    
    // Simulate "Application Memory" filling up
    private static readonly List<byte[]> MemoryLeakStore = new();

    public BufferSaturationWorker(ILogger<BufferSaturationWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration.GetConnectionString("RabbitMq") ?? "amqp://guest:guest@localhost:5672"),
            DispatchConsumersAsync = true
        };

        try 
        {
            // Simple retry logic
            var retries = 5;
            while (retries > 0)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    break;
                }
                catch
                {
                    retries--;
                    Thread.Sleep(2000);
                }
            }

            _channel.QueueDeclare(queue: "saturation-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // CRITICAL: High Prefetch count. 
            // This pulls messages from Broker -> TCP Buffer -> Application Memory
            // If we don't process them, they sit in the Client Buffer (Memory).
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 5000, global: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                
                // SCENARIO: We "Receive" fast, but we "Leak" memory.
                // We are NOT Acking immediately? Or we Ack to get MORE data to crash memory?
                // Let's Ack to keep pulling from TCP.
                MemoryLeakStore.Add(body);

                if (MemoryLeakStore.Count % 1000 == 0)
                {
                    var mem = GC.GetTotalMemory(false) / 1024 / 1024;
                    _logger.LogWarning($"[BufferSaturation] Stored {MemoryLeakStore.Count} msgs. App Memory: {mem} MB. Messages are now in App RAM (Heap), not Broker Disk.");
                }

                // Simulate slight processing overhead but much faster than production
                await Task.Delay(1, stoppingToken); 
                
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "saturation-queue", autoAck: false, consumer: consumer);
            _logger.LogInformation("BufferSaturationWorker listening on 'saturation-queue' with Prefetch=5000");
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Failed to start BufferSaturationWorker");
        }

        return Task.CompletedTask;
    }
}
