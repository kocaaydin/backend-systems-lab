using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("")]
public sealed class InventoryController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { service = "inventory-api", status = "ok" });
    }

    [HttpGet("inventory/stock/{sku}")]
    public async Task<IActionResult> Stock([FromServices] NpgsqlDataSource ds, string sku, CancellationToken ct)
    {
        await using var conn = await ds.OpenConnectionAsync(ct);
        var cmd = new NpgsqlCommand("SELECT sku, available FROM stock WHERE sku=@sku", conn);
        cmd.Parameters.AddWithValue("sku", sku);

        await using var r = await cmd.ExecuteReaderAsync(ct);
        if (!await r.ReadAsync(ct))
        {
            return NotFound();
        }

        return Ok(new { sku = r.GetString(0), available = r.GetInt32(1) });
    }

    [HttpGet("inventory/reservations")]
    public async Task<IActionResult> Reservations([FromServices] NpgsqlDataSource ds, CancellationToken ct)
    {
        var list = new List<object>();
        await using var conn = await ds.OpenConnectionAsync(ct);
        var cmd = new NpgsqlCommand("SELECT id, order_id, sku, quantity, status, created_at FROM reservations ORDER BY id DESC LIMIT 50", conn);

        await using var r = await cmd.ExecuteReaderAsync(ct);
        while (await r.ReadAsync(ct))
        {
            list.Add(new
            {
                id = r.GetInt64(0),
                orderId = r.GetInt64(1),
                sku = r.GetString(2),
                quantity = r.GetInt32(3),
                status = r.GetString(4),
                createdAt = r.GetDateTime(5)
            });
        }

        return Ok(list);
    }
}
