using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for specific protocols
builder.WebHost.ConfigureKestrel(options =>
{
    // Port 5001: HTTP/1.1 Only
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });

    // Port 5002: HTTP/2 Only (H2C - No TLS)
    options.ListenAnyIP(5002, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
    
    // Port 5003: gRPC Placeholder (HTTP/2)
    options.ListenAnyIP(5003, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

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
