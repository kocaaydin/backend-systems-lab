using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // HTTP/1.1 endpoint for Keep-Alive ON/OFF comparison
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });

    // HTTP/2 + TLS endpoint for multiplexing comparison
    options.ListenAnyIP(5002, listenOptions =>
    {
        listenOptions.UseHttps("/https/tls.pfx", "changeit");
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    // HTTP/1.1 + TLS endpoint for fair TLS comparison against HTTP/2
    options.ListenAnyIP(5003, listenOptions =>
    {
        listenOptions.UseHttps("/https/tls.pfx", "changeit");
        listenOptions.Protocols = HttpProtocols.Http1;
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
