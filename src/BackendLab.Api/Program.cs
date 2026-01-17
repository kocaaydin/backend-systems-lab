using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// REGISTER SERVICES
builder.Services.AddTransient<ClientRateLimitingHandler>(sp => new ClientRateLimitingHandler(100)); // Limit 100 RPS
builder.Services.AddHttpClient("RateLimitedClient").AddHttpMessageHandler<ClientRateLimitingHandler>();
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

// Experiment 3: Outgoing HTTP Concurrency Limit
app.MapGet("/experiments/http-limit", async (int? limit, IConfiguration config) => 
{
    var maxConn = limit ?? 10; // Default low limit to show waiting
    var externalUrl = config["ExternalServiceUrl"] ?? "http://localhost:8081";

    // WARNING: Creating HttpClient per request is usually BAD practice (socket exhaustion).
    // But here we do it to dynamically change 'MaxConnectionsPerServer' for the demo.
    // In a real fix, this handler would be a Singleton with high limit.
    using var handler = new SocketsHttpHandler
    {
        MaxConnectionsPerServer = maxConn,
        PooledConnectionLifetime = TimeSpan.FromMinutes(2)
    };

    using var client = new HttpClient(handler);
    
    var sw = System.Diagnostics.Stopwatch.StartNew();
    try 
    {
        // Call the external service
        var response = await client.GetStringAsync(externalUrl);
        sw.Stop();
        return Results.Ok(new { 
            Status = "Success", 
            LatencyMs = sw.ElapsedMilliseconds, 
            UsedLimit = maxConn,
            ExternalData = response.Trim()
        });
    }
    catch (Exception ex)
    {
        sw.Stop();
        return Results.Problem($"External Call Failed: {ex.Message} (Latency: {sw.ElapsedMilliseconds}ms)");
    }
});

// Experiment 3.2: Outbound Rate Limiter (DelegatingHandler Implementation)
// This is the "Correct Way" to implement outgoing limits transparently.


app.MapGet("/experiments/rate-limit", async (IHttpClientFactory clientFactory, IConfiguration config) => 
{
    var externalUrl = config["ExternalServiceUrl"] ?? "http://localhost:8081";
    
    // Get the client with the handler attached
    var client = clientFactory.CreateClient("RateLimitedClient");
    
    try 
    {
        var response = await client.GetAsync(externalUrl);
        
        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            return Results.StatusCode(429); // Propagate the limit
        }

        var content = await response.Content.ReadAsStringAsync();
        return Results.Ok(new { Status = "Allowed & Forwarded", ExternalData = content.Trim() });
    }
    catch (Exception ex)
    {
        return Results.Problem($"External Call Failed: {ex.Message}");
    }
});

// Experiment 3.3: Socket Exhaustion (Bad HttpClient Usage)
app.MapGet("/experiments/bad-http-client", async (IConfiguration config) => 
{
    var externalUrl = config["ExternalServiceUrl"] ?? "http://localhost:8081";
    
    // BAD PRACTICE: Creating new HttpClient per request!
    // This will leave many sockets in TIME_WAIT state, eventually running out of ports.
    // Do NOT wrap in 'using' to make it even worse (optional, but shows memory leak too), 
    // but even with 'using', the socket stays in TIME_WAIT by OS.
    using var client = new HttpClient(); 
    
    try 
    {
        var response = await client.GetStringAsync(externalUrl);
        return Results.Ok(new { Status = "Success", Length = response.Length });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed: {ex.Message}");
    }
});

app.MapMetrics();

app.Run();

class ClientRateLimitingHandler : DelegatingHandler
{
    private readonly int _limit;
    private int _requestCount = 0;
    private DateTime _lastResetTime = DateTime.UtcNow;
    private readonly object _lock = new object();

    public ClientRateLimitingHandler(int limit)
    {
        _limit = limit;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastResetTime).TotalSeconds >= 1)
            {
                _requestCount = 0;
                _lastResetTime = now;
            }

            if (_requestCount >= _limit)
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.TooManyRequests));
            }

            _requestCount++;
        }

        return base.SendAsync(request, cancellationToken);
    }
}
