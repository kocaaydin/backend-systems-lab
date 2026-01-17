using BackendLab.Worker;
using Prometheus;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<KafkaWorker>();

// Start metric server
var metricServer = new KestrelMetricServer(port: 8080);
metricServer.Start();

var host = builder.Build();
host.Run();
