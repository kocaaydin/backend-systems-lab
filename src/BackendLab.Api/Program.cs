using Prometheus;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.UseHttpMetrics();

app.MapGet("/experiments/latency", async () => 
{
    await Task.Delay(200);
    return Results.Ok(new { Service = "BackendLab.Api", Status = "Online", Version = "1.0.0" });
});

app.MapMetrics();

app.Run();
