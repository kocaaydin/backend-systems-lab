using System.Text.Json;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public sealed class InventoryConsumer : BackgroundService
{
    private readonly ILogger<InventoryConsumer> _logger;
    private readonly NpgsqlDataSource _ds;
    private readonly string _rabbitHost;

    public InventoryConsumer(ILogger<InventoryConsumer> logger, NpgsqlDataSource ds)
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
                channel.QueueDeclare("inventory.order.created.dlq", durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind("inventory.order.created.dlq", "events.dlx", "inventory.order.created.failed");

                var mainArgs = new Dictionary<string, object>
                {
                    ["x-dead-letter-exchange"] = "events.dlx",
                    ["x-dead-letter-routing-key"] = "inventory.order.created.failed"
                };
                channel.QueueDeclare("inventory.order.created", durable: true, exclusive: false, autoDelete: false, arguments: mainArgs);
                channel.QueueBind("inventory.order.created", "events", "order.created");
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

                        await using var dbConn = await _ds.OpenConnectionAsync(stoppingToken);
                        await using var tx = await dbConn.BeginTransactionAsync(stoppingToken);

                        var update = new NpgsqlCommand(@"
                            UPDATE stock
                            SET available = available - @q
                            WHERE sku=@sku AND available >= @q", dbConn, tx);
                        update.Parameters.AddWithValue("sku", evt.Sku);
                        update.Parameters.AddWithValue("q", evt.Quantity);
                        var affected = await update.ExecuteNonQueryAsync(stoppingToken);

                        var status = affected > 0 ? "RESERVED" : "INSUFFICIENT_STOCK";

                        var reserve = new NpgsqlCommand(@"
                            INSERT INTO reservations(order_id, sku, quantity, status)
                            VALUES(@o, @sku, @q, @s)
                            ON CONFLICT (order_id) DO NOTHING", dbConn, tx);
                        reserve.Parameters.AddWithValue("o", evt.OrderId);
                        reserve.Parameters.AddWithValue("sku", evt.Sku);
                        reserve.Parameters.AddWithValue("q", evt.Quantity);
                        reserve.Parameters.AddWithValue("s", status);
                        await reserve.ExecuteNonQueryAsync(stoppingToken);

                        await tx.CommitAsync(stoppingToken);

                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                        _logger.LogInformation("Inventory processed. orderId={OrderId}, status={Status}", evt.OrderId, status);
                    }
                    catch (Exception ex)
                    {
                        channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                        _logger.LogWarning(ex, "Inventory consumer message islerken hata");
                    }
                };

                channel.BasicConsume("inventory.order.created", autoAck: false, consumer);
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(300, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Inventory consumer baglanamadi, retry");
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
