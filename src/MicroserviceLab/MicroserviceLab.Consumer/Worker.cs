using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Confluent.Kafka;

namespace MicroserviceLab.Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MicroserviceLab Consumer Started");

        // Start RabbitMQ Consumer
        _ = Task.Run(() => ConsumeRabbitMQ(stoppingToken), stoppingToken);

        // Start Kafka Consumer
        _ = Task.Run(() => ConsumeKafka(stoppingToken), stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private void ConsumeRabbitMQ(CancellationToken stoppingToken)
    {
        var connectionString = _config["ConnectionStrings:RabbitMq"] ?? "amqp://guest:guest@rabbitmq:5672";
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "microservice-lab-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            ActivityContext parentContext = default;
            if (ea.BasicProperties.Headers != null && ea.BasicProperties.Headers.ContainsKey("traceparent"))
            {
                var traceParent = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["traceparent"]);
                ActivityContext.TryParse(traceParent, null, out parentContext);
            }

            using var activity = new ActivitySource("MicroserviceLab.Consumer").StartActivity("ProcessRabbitMQMessage", ActivityKind.Consumer, parentContext);

            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            activity?.SetTag("messaging.system", "rabbitmq");
            activity?.SetTag("messaging.destination", "microservice-lab-queue");

            _logger.LogInformation("[RabbitMQ] Received: {Message} | TraceId: {TraceId}", message, activity?.TraceId);
            Thread.Sleep(100);
        };

        channel.BasicConsume(queue: "microservice-lab-queue", autoAck: true, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            Thread.Sleep(1000);
        }
    }

    private void ConsumeKafka(CancellationToken stoppingToken)
    {
        var bootstrapServers = _config["ConnectionStrings:Kafka"] ?? "kafka:29092";
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "microservice-lab-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("microservice-lab-topic");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);

                ActivityContext parentContext = default;
                if (consumeResult.Message.Headers.TryGetLastBytes("traceparent", out var traceParentBytes))
                {
                    var traceParent = Encoding.UTF8.GetString(traceParentBytes);
                    ActivityContext.TryParse(traceParent, null, out parentContext);
                }

                using var activity = new ActivitySource("MicroserviceLab.Consumer").StartActivity("ProcessKafkaMessage", ActivityKind.Consumer, parentContext);

                activity?.SetTag("messaging.system", "kafka");
                activity?.SetTag("messaging.destination", "microservice-lab-topic");

                _logger.LogInformation("[Kafka] Received: {Value} | TraceId: {TraceId}", consumeResult.Message.Value, activity?.TraceId);
                Thread.Sleep(50);
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}
