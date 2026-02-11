using ThreadTypesApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<CpuCalculator>();
builder.Services.AddSingleton<WorkQueue>();
builder.Services.AddHostedService<QueueWorkerService>();

var app = builder.Build();

app.MapControllers();
app.Run("http://0.0.0.0:8091");
