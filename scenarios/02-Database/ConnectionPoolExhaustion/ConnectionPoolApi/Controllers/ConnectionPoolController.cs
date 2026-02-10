using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("connection-pool")]
public sealed class ConnectionPoolController : ControllerBase
{
    [HttpPost("exhaust")]
    public async Task<IActionResult> Exhaust([FromServices] SqlConnectionFactory connectionFactory, [FromQuery] int durationMs = 2000, CancellationToken ct = default)
    {
        var safeDuration = Math.Clamp(durationMs, 100, 15000);

        await using var conn = connectionFactory.Create();
        await conn.OpenAsync(ct);

        await using var cmd = conn.CreateCommand();
        cmd.CommandTimeout = 30;
        cmd.CommandText = $"WAITFOR DELAY '00:00:{safeDuration / 1000:D2}'; SELECT @@SPID;";

        var spid = await cmd.ExecuteScalarAsync(ct);

        return Ok(new
        {
            heldMs = safeDuration,
            spid,
            note = "Connection held to simulate pool pressure"
        });
    }

    [HttpGet("status")]
    public async Task<IActionResult> Status([FromServices] SqlConnectionFactory connectionFactory, CancellationToken ct)
    {
        await using var conn = connectionFactory.Create();
        await conn.OpenAsync(ct);

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT COUNT(*)
            FROM sys.dm_exec_sessions
            WHERE is_user_process = 1
              AND program_name LIKE '%Core .Net SqlClient Data Provider%';";

        var countObj = await cmd.ExecuteScalarAsync(ct);
        var openConnections = Convert.ToInt32(countObj);

        return Ok(new
        {
            openConnections,
            maxPoolHint = 10
        });
    }
}
