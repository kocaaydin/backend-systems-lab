using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var db = Environment.GetEnvironmentVariable("PAYMENT_DB") ?? "Host=payment-db;Port=5432;Database=paymentdb;Username=payment;Password=paymentpwd";

builder.Services.AddControllers();
builder.Services.AddSingleton(new NpgsqlDataSourceBuilder(db).Build());
builder.Services.AddHostedService<PaymentConsumer>();

var app = builder.Build();
app.MapControllers();
app.Run("http://0.0.0.0:8080");
