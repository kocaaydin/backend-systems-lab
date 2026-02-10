using System.Text;
using Npgsql;
using RabbitMQ.Client;

public sealed class OutboxPublisher : BackgroundService
{
    private readonly ILogger<OutboxPublisher> _logger;
    private readonly NpgsqlDataSource _dataSource;
    private readonly string _rabbitHost;

    public OutboxPublisher(ILogger<OutboxPublisher> logger, NpgsqlDataSource dataSource)
    {
        _logger = logger;
        _dataSource = dataSource;
        _rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PublishBatchAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Outbox publish denemesi basarisiz, tekrar denenecek");
            }

            await Task.Delay(1500, stoppingToken);
        }
    }

    private async Task PublishBatchAsync(CancellationToken ct)
    {
        var events = new List<(long Id, string EventType, string Payload)>();

        await using (var conn = await _dataSource.OpenConnectionAsync(ct))
        {
            var pick = new NpgsqlCommand(@"
                SELECT id, event_type, payload
                FROM outbox_events
                WHERE published_at IS NULL
                ORDER BY id
                LIMIT 50", conn);

            await using var r = await pick.ExecuteReaderAsync(ct);
            while (await r.ReadAsync(ct))
            {
                events.Add((r.GetInt64(0), r.GetString(1), r.GetString(2)));
            }
        }

        if (events.Count == 0)
        {
            return;
        }

        var factory = new ConnectionFactory { HostName = _rabbitHost };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "events", type: ExchangeType.Topic, durable: true);

        await using var markConn = await _dataSource.OpenConnectionAsync(ct);
        foreach (var evt in events)
        {
            var body = Encoding.UTF8.GetBytes(evt.Payload);
            var props = channel.CreateBasicProperties();
            props.Persistent = true;
            props.ContentType = "application/json";
            channel.BasicPublish(exchange: "events", routingKey: evt.EventType, basicProperties: props, body: body);

            var mark = new NpgsqlCommand("UPDATE outbox_events SET published_at=now() WHERE id=@id", markConn);
            mark.Parameters.AddWithValue("id", evt.Id);
            await mark.ExecuteNonQueryAsync(ct);
        }

        _logger.LogInformation("Outbox publish tamamlandi. Adet={Count}", events.Count);
    }
}
