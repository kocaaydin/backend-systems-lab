using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var db = Environment.GetEnvironmentVariable("INVENTORY_DB") ?? "Host=inventory-db;Port=5432;Database=inventorydb;Username=inventory;Password=inventorypwd";

builder.Services.AddControllers();
builder.Services.AddSingleton(new NpgsqlDataSourceBuilder(db).Build());
builder.Services.AddHostedService<InventoryConsumer>();

var app = builder.Build();
app.MapControllers();
app.Run("http://0.0.0.0:8080");
