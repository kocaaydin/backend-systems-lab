using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BackendLab.Api.Controllers;

[ApiController]
[Route("connection-pool")]
public class ConnectionPoolController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<ConnectionPoolController> _logger;
    private static int _activeConnections = 0;

    public ConnectionPoolController(
        IConfiguration config,
        ILogger<ConnectionPoolController> logger)
    {
        _config = config;
        _logger = logger;
    }

    [HttpPost("exhaust")]
    public async Task<IActionResult> Exhaust([FromQuery] int durationMs = 5000)
    {
        var connectionString = _config.GetConnectionString("DefaultConnection") ?? _config["ConnectionStrings:DefaultConnection"];

        if (string.IsNullOrEmpty(connectionString))
        {
            return Problem("Connection string 'DefaultConnection' not found.");
        }

        // Track unique request ID for logs
        var reqId = HttpContext.TraceIdentifier.Substring(0, 8);
        
        try
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            _logger.LogInformation($"[Pool] {reqId} - Want connection...");

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            sw.Stop();

            var currentCount = Interlocked.Increment(ref _activeConnections);
            
            // IF wait time > 100ms, it means we queued! highlight this.
             if (sw.ElapsedMilliseconds > 100)
            {
                _logger.LogWarning($"[Pool] {reqId} - QUEUED! Waited: {sw.ElapsedMilliseconds}ms. Connection Acquired. (Active: {currentCount})");
            }
            else
            {
                _logger.LogInformation($"[Pool] {reqId} - Instant! Waited: {sw.ElapsedMilliseconds}ms. Connection Acquired. (Active: {currentCount})");
            }

            // Simulate work holding the connection
            await Task.Delay(durationMs);

            Interlocked.Decrement(ref _activeConnections);
            _logger.LogInformation($"[Pool] {reqId} - Releasing connection.");
            
            return Ok(new { Status = "Connection held and released", DurationMs = durationMs, WaitTimeMs = sw.ElapsedMilliseconds });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to open connection");
            return Problem(ex.Message);
        }
    }

    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var connectionString = _config.GetConnectionString("DefaultConnection") ?? _config["ConnectionStrings:DefaultConnection"];
         if (string.IsNullOrEmpty(connectionString))
        {
            return Problem("Connection string 'DefaultConnection' not found.");
        }

        try
        {
            // Use a separate connection string with no pooling to ensure we can always connect to check status
            var simpleBuilder = new SqlConnectionStringBuilder(connectionString);
            simpleBuilder.Pooling = false; 
            // Optional: Set a short timeout for the status check
            simpleBuilder.ConnectTimeout = 5;

            using var connection = new SqlConnection(simpleBuilder.ConnectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            // Count sessions from .NET applications
            command.CommandText = @"
                SELECT COUNT(*) 
                FROM sys.dm_exec_sessions 
                WHERE program_name LIKE '%Core .Net SqlClient Data Provider%' 
                   OR program_name LIKE '%BackendLab%'
            ";
            
            var count = (int?)await command.ExecuteScalarAsync();
            return Ok(new { OpenConnections = count ?? 0 });
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Failed to check status");
            return Problem(ex.Message);
        }
    }
}
