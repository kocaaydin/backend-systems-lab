using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueLab.Worker;

public class PoisonWorker : BackgroundService
{
    private readonly ILogger<PoisonWorker> _logger;
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;

    public PoisonWorker(ILogger<PoisonWorker> logger, IConfiguration configuration)
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
            // Simple retry logic for startup
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

            _channel.QueueDeclare(queue: "poison-queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                _logger.LogInformation($"[PoisonWorker] Received: {message}");

                if (message.Contains("POISON"))
                {
                    _logger.LogError("POISON MESSAGE DETECTED! CRASHING/FAILING...");
                    // Throwing exception here to trigger re-queue if autoAck is false or simply failing processing
                    // If we don't ack, and channel closes or we throw, it might be redelivered indefinitely.
                    // For demo, we simulate a crash loop by sleeping and then throwing.
                    await Task.Delay(1000, stoppingToken); 
                    throw new Exception("Poison message encountered!");
                }

                await Task.Delay(100, stoppingToken); // Simulate work
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            // autoAck: false -> we must manually ack. If we crash before ack, it requeues.
            _channel.BasicConsume(queue: "poison-queue",
                                 autoAck: false, 
                                 consumer: consumer);
            
            _logger.LogInformation("PoisonWorker listening on 'poison-queue'");
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Failed to start PoisonWorker");
        }

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
