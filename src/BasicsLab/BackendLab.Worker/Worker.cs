using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackendLab.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker Service Started - Listening for Messages...");

        try 
        {
            var factory = new ConnectionFactory { Uri = new Uri(Environment.GetEnvironmentVariable("ConnectionStrings__RabbitMq") ?? "amqp://guest:guest@rabbitmq:5672") };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "microservice-lab-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                // EXTRACT TRACE CONTEXT
                ActivityContext parentContext = default;
                if (ea.BasicProperties.Headers != null && 
                    ea.BasicProperties.Headers.ContainsKey("traceparent"))
                {
                    // Basic extraction of W3C Trace Parent
                    var traceParent = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["traceparent"]);
                    ActivityContext.TryParse(traceParent, null, out parentContext);
                }

                // START CONSUMER ACTIVITY
                using var activity = new ActivitySource("BackendLab.Worker").StartActivity("ProcessMessage", ActivityKind.Consumer, parentContext);
                
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                activity?.SetTag("messaging.system", "rabbitmq");
                activity?.SetTag("messaging.destination", "microservice-lab-queue");
                
                _logger.LogInformation(" [x] Received {Message} | TraceId: {TraceId}", message, activity?.TraceId);

                // Simulate work
                Thread.Sleep(100); 
            };

            channel.BasicConsume(queue: "microservice-lab-queue", autoAck: true, consumer: consumer);

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Worker failed to start RabbitMQ consumer.");
        }
    }
}
