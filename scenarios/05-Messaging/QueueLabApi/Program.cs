var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<QueueLabState>();
builder.Services.AddHostedService<BackpressureWorker>();
builder.Services.AddHostedService<PoisonWorker>();
builder.Services.AddHostedService<HolWorker>();
builder.Services.AddHostedService<TcpLabWorker>();

var app = builder.Build();

app.MapControllers();
app.Run("http://0.0.0.0:8090");
