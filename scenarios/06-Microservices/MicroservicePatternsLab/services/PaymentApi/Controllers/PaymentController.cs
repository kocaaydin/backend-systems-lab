using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("")]
public sealed class PaymentController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { service = "payment-api", status = "ok" });
    }

    [HttpGet("payments")]
    public async Task<IActionResult> List([FromServices] NpgsqlDataSource ds, CancellationToken ct)
    {
        var result = new List<object>();
        await using var conn = await ds.OpenConnectionAsync(ct);
        var cmd = new NpgsqlCommand("SELECT id, order_id, amount, status, created_at FROM payments ORDER BY id DESC LIMIT 50", conn);

        await using var r = await cmd.ExecuteReaderAsync(ct);
        while (await r.ReadAsync(ct))
        {
            result.Add(new
            {
                id = r.GetInt64(0),
                orderId = r.GetInt64(1),
                amount = r.GetDecimal(2),
                status = r.GetString(3),
                createdAt = r.GetDateTime(4)
            });
        }

        return Ok(result);
    }
}
