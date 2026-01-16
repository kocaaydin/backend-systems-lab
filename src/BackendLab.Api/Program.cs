using Prometheus;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.UseHttpMetrics();

app.MapGet("/health", () => Results.Ok("OK"));

app.MapGet("/experiments/latency", async () => 
{
    await Task.Delay(200);
    return Results.Ok(new { Service = "BackendLab.Api", Status = "Online", Version = "1.0.0" });
});

app.MapGet("/experiments/cpu", (int? n) => 
{
    var count = n ?? 10000;
    // CPU Killer: Find prime numbers via brute force
    int primes = 0;
    for (int i = 2; i < count; i++)
    {
        bool isPrime = true;
        for (int j = 2; j <= Math.Sqrt(i); j++)
        {
            if (i % j == 0) 
            {
                isPrime = false;
                break;
            }
        }
        if (isPrime) primes++;
    }
    return Results.Ok(new { PrimesFound = primes, TestedUpTo = count });
});

app.MapMetrics();

app.Run();
