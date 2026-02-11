using ThreadTypesApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<CpuCalculator>();

var app = builder.Build();

app.MapControllers();
app.Run("http://0.0.0.0:8091");
