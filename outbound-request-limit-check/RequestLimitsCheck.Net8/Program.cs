using System.Collections.Concurrent;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// High Performance HttpClient Setup
builder.Services.AddSingleton<HttpClient>(sp => 
{
    var handler = new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        MaxConnectionsPerServer = int.MaxValue,
        EnableMultipleHttp2Connections = true
    };
    return new HttpClient(handler);
});

var app = builder.Build();

app.MapGet("/benchmark", async (int? count, HttpClient client, IConfiguration config) => 
{
    var targetUrl = config["EXTERNAL_URL"] ?? "http://localhost:8081";
    int loopCount = count ?? 2000;
    int successCount = 0;
    int failCount = 0;

    var logs = new ConcurrentQueue<string>();
    var tasks = new List<Task>(loopCount);

    var sw = Stopwatch.StartNew();
    Console.WriteLine($"Starting Batch of {loopCount} requests...");

    for (int i = 0; i < loopCount; i++)
    {
        tasks.Add(Task.Run(async () => 
        {
            try 
            {
                using var response = await client.GetAsync(targetUrl);
                var content = await response.Content.ReadAsStringAsync();
                
                // Capture elapsed time relative to start of batch
                var elapsed = sw.ElapsedMilliseconds;
                
                if (response.IsSuccessStatusCode)
                {
                    Interlocked.Increment(ref successCount);
                    logs.Enqueue($"[{elapsed}ms] OK | GUID: {content}");
                }
                else
                {
                    Interlocked.Increment(ref failCount);
                    logs.Enqueue($"[{elapsed}ms] FAIL | {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref failCount);
                logs.Enqueue($"[{sw.ElapsedMilliseconds}ms] EXCEPTION | {ex.Message}");
            }
        }));
    }

    await Task.WhenAll(tasks);
    sw.Stop();

    Console.WriteLine($"Batch Finished in {sw.ElapsedMilliseconds}ms. Dumping Logs...");
    foreach (var log in logs)
    {
        Console.WriteLine(log);
    }
    Console.WriteLine($"Batch Summary: Success={successCount}, Fail={failCount}, TotalTime={sw.ElapsedMilliseconds}ms");

    return Results.Ok(new 
    { 
        Time = DateTime.Now, 
        Success = successCount, 
        Fail = failCount, 
        TotalTimeMs = sw.ElapsedMilliseconds,
        Target = targetUrl 
    });
});

app.Run();
