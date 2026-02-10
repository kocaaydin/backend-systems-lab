using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var db = Environment.GetEnvironmentVariable("ORDER_DB") ?? "Host=order-db;Port=5432;Database=orderdb;Username=order;Password=orderpwd";

builder.Services.AddControllers();
builder.Services.AddSingleton(new NpgsqlDataSourceBuilder(db).Build());
builder.Services.AddHostedService<OutboxPublisher>();

var app = builder.Build();
app.MapControllers();
app.Run("http://0.0.0.0:8080");
