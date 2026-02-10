using System.Text.Json;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public sealed class PaymentConsumer : BackgroundService
{
    private readonly ILogger<PaymentConsumer> _logger;
    private readonly NpgsqlDataSource _ds;
    private readonly string _rabbitHost;

    public PaymentConsumer(ILogger<PaymentConsumer> logger, NpgsqlDataSource ds)
    {
        _logger = logger;
        _ds = ds;
        _rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = _rabbitHost };
                using var conn = factory.CreateConnection();
                using var channel = conn.CreateModel();
                channel.ExchangeDeclare("events", ExchangeType.Topic, durable: true);
                channel.ExchangeDeclare("events.dlx", ExchangeType.Topic, durable: true);
                channel.QueueDeclare("payment.order.created.dlq", durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind("payment.order.created.dlq", "events.dlx", "payment.order.created.failed");

                var mainArgs = new Dictionary<string, object>
                {
                    ["x-dead-letter-exchange"] = "events.dlx",
                    ["x-dead-letter-routing-key"] = "payment.order.created.failed"
                };
                channel.QueueDeclare("payment.order.created", durable: true, exclusive: false, autoDelete: false, arguments: mainArgs);
                channel.QueueBind("payment.order.created", "events", "order.created");
                channel.BasicQos(prefetchSize: 0, prefetchCount: 20, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (_, ea) =>
                {
                    try
                    {
                        var payload = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                        var evt = JsonSerializer.Deserialize<OrderCreatedEvent>(payload);
                        if (evt is null)
                        {
                            return;
                        }

                        await Task.Delay(80, stoppingToken);

                        await using var dbConn = await _ds.OpenConnectionAsync(stoppingToken);
                        var cmd = new NpgsqlCommand(@"
                            INSERT INTO payments(order_id, amount, status)
                            VALUES(@o, @a, 'AUTHORIZED')
                            ON CONFLICT (order_id) DO NOTHING", dbConn);
                        cmd.Parameters.AddWithValue("o", evt.OrderId);
                        cmd.Parameters.AddWithValue("a", evt.TotalAmount);
                        await cmd.ExecuteNonQueryAsync(stoppingToken);

                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                        _logger.LogInformation("Payment authorized. orderId={OrderId}", evt.OrderId);
                    }
                    catch (Exception ex)
                    {
                        channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                        _logger.LogWarning(ex, "Payment consumer message islerken hata");
                    }
                };

                channel.BasicConsume("payment.order.created", autoAck: false, consumer);
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(300, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Payment consumer baglanamadi, retry");
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
