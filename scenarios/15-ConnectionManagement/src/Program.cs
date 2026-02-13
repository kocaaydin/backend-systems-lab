using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello form Connection Management!");

// A simple endpoint to test latency
app.MapGet("/api/benchmark/fast", () => Results.Ok(new { message = "Fast response", time = DateTime.UtcNow }));

app.Run();
