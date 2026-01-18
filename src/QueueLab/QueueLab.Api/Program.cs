using QueueLab.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Backpressure Demo Services
builder.Services.AddSingleton<BackpressureChannel>();
builder.Services.AddHostedService<BackpressureWorker>();

// Head of Line Blocking Services
builder.Services.AddSingleton<HolChannel>();
builder.Services.AddHostedService<HolWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
