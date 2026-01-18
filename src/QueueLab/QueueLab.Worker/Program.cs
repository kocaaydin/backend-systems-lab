using QueueLab.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<PoisonWorker>();
builder.Services.AddHostedService<RebalanceWorker>();
builder.Services.AddHostedService<BufferSaturationWorker>();
builder.Services.AddHostedService<BrokerVsSocketWorker>();
builder.Services.AddHostedService<ConnectionChurnWorker>();

var host = builder.Build();
host.Run();
