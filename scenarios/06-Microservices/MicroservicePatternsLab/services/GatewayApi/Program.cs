var builder = WebApplication.CreateBuilder(args);

var orderApiUrl = Environment.GetEnvironmentVariable("ORDER_API_URL") ?? "http://order-api:8080";

builder.Services.AddControllers();
builder.Services.AddHttpClient("orders", c =>
{
    c.BaseAddress = new Uri(orderApiUrl);
    c.Timeout = TimeSpan.FromSeconds(5);
});
builder.Services.AddScoped<IOrderClient, OrderClient>();

var app = builder.Build();
app.MapControllers();
app.Run("http://0.0.0.0:8080");
