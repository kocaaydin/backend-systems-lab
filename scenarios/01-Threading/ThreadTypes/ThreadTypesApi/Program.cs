using ThreadTypesApi.Services;
using ThreadTypesApi.Services.Gc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<CpuCalculator>();
builder.Services.AddSingleton<GcLab>();

var app = builder.Build();

app.MapControllers();
app.Run("http://0.0.0.0:8091");
