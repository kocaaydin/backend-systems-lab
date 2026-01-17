
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Diagnostics;

namespace ResilienceLab.Api.Controllers;

// --- 1. DB Contexts ---

public class ResilienceContext : DbContext
{
    public ResilienceContext(DbContextOptions<ResilienceContext> options) : base(options) { }
    // No specific DbSets needed for connection test
}

// --- 2. Controller ---

[ApiController]
[Route("experiments/resilience/db")]
public class DbResilienceController : ControllerBase
{
    private readonly ILogger<DbResilienceController> _logger;

    public DbResilienceController(ILogger<DbResilienceController> logger)
    {
        _logger = logger;
    }

    [HttpGet("connect")]
    public async Task<IActionResult> TestDbConnection([FromQuery] bool useRetry = false, [FromQuery] string host = "non-existent-host")
    {
        // Dynamic construction of DbContext to demonstrate configuration difference at runtime
        var builder = new DbContextOptionsBuilder<ResilienceContext>();
        var connectionString = $"Host={host};Database=postgres;Username=guest;Password=guest;Timeout=2"; // Short timeout

        if (useRetry)
        {
            builder.UseNpgsql(connectionString, options => 
            {
                // EXPERIMENT: ENABLE RETRY ON FAILURE
                options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(1),
                    errorCodesToAdd: null);
            });
        }
        else
        {
            builder.UseNpgsql(connectionString); // No retry strategy
        }

        using var context = new ResilienceContext(builder.Options);
        var sw = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation("Attempting DB Connection. Retry Enabled: {Retry}. Host: {Host}", useRetry, host);
            
            // Just try to open connection
            await context.Database.OpenConnectionAsync();
            
            sw.Stop();
            return Ok($"Connected successfully in {sw.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError("DB Connection Failed after {Duration}ms. Error: {Error}", sw.ElapsedMilliseconds, ex.Message);
            
            return StatusCode(500, new 
            { 
                Status = "Failed", 
                RetryEnabled = useRetry, 
                DurationMs = sw.ElapsedMilliseconds, 
                Exception = ex.Message 
            });
        }
    }
}
