using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueLabApi.Workers;

public sealed class RabbitMqWorker : BackgroundService
{
    private readonly ILogger<RabbitMqWorker> _logger;
    private readonly IConfiguration _configuration;
    private const string QueueName = "lab_backpressure_rabbit";

    public RabbitMqWorker(ILogger<RabbitMqWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var hostName = _configuration["RabbitMqHost"] ?? "localhost";
        var factory = new ConnectionFactory { HostName = hostName };
        
        // Connection retry loop
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                // BACKPRESSURE NOKTASI: Prefetch Count = 5
                // Consumer aynı anda en fazla 5 onaysız (unacked) mesaj alabilir.
                channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    // Senkronize bir gecikme simülasyonu (async event handler RabbitMQ.Client eski sürümlerinde risklidir, burada Thread.Sleep veya Task.Wait kullanımı demo amaçlıdır)
                    // Gerçek senaryoda AsyncEventingBasicConsumer kullanılır.
                    Task.Delay(100).Wait(); 

                    // İşlem bitti, RabbitMQ'ya onay ver.
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

                // Servis kapanana kadar bekle
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("RabbitMQ waiting... ({Message})", ex.Message);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}