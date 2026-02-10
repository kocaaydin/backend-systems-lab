using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? "Server=localhost,1435;Database=master;User Id=sa;Password=VeryStrongPassword123!;TrustServerCertificate=True;Max Pool Size=10;";

builder.Services.AddControllers();
builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));

var app = builder.Build();
app.MapControllers();
app.Run("http://0.0.0.0:8080");
