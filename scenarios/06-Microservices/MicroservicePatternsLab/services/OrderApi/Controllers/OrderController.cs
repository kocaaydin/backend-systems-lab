using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("")]
public sealed class OrderController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { service = "order-api", status = "ok" });
    }

    [HttpPost("orders")]
    public async Task<IResult> Create([FromServices] NpgsqlDataSource dataSource, [FromBody] OrderCreateRequest request, CancellationToken ct)
    {
        var idemKey = Request.Headers["Idempotency-Key"].ToString();
        if (string.IsNullOrWhiteSpace(idemKey))
        {
            return Results.BadRequest(new { error = "Idempotency-Key header zorunlu" });
        }

        await using var conn = await dataSource.OpenConnectionAsync(ct);
        await using var tx = await conn.BeginTransactionAsync(ct);

        var existingCmd = new NpgsqlCommand("SELECT order_id FROM idempotency_keys WHERE idem_key=@k", conn, tx);
        existingCmd.Parameters.AddWithValue("k", idemKey);
        var existing = await existingCmd.ExecuteScalarAsync(ct);
        if (existing is long existingOrderId)
        {
            await tx.RollbackAsync(ct);
            return Results.Ok(new { orderId = existingOrderId, deduplicated = true });
        }

        var totalAmount = request.Quantity * request.UnitPrice;

        var insertOrder = new NpgsqlCommand(@"
            INSERT INTO orders(customer_id, sku, quantity, unit_price, total_amount, status)
            VALUES(@c, @s, @q, @u, @t, 'CREATED')
            RETURNING id", conn, tx);

        insertOrder.Parameters.AddWithValue("c", request.CustomerId);
        insertOrder.Parameters.AddWithValue("s", request.Sku);
        insertOrder.Parameters.AddWithValue("q", request.Quantity);
        insertOrder.Parameters.AddWithValue("u", request.UnitPrice);
        insertOrder.Parameters.AddWithValue("t", totalAmount);

        var orderIdObj = await insertOrder.ExecuteScalarAsync(ct);
        var orderId = Convert.ToInt64(orderIdObj);

        var evt = new OrderCreatedEvent(orderId, request.CustomerId, request.Sku, request.Quantity, request.UnitPrice, totalAmount, DateTime.UtcNow);
        var payload = JsonSerializer.Serialize(evt);

        var insertOutbox = new NpgsqlCommand(@"
            INSERT INTO outbox_events(event_type, payload)
            VALUES('order.created', @p)", conn, tx);
        insertOutbox.Parameters.AddWithValue("p", payload);
        await insertOutbox.ExecuteNonQueryAsync(ct);

        var insertIdem = new NpgsqlCommand(@"
            INSERT INTO idempotency_keys(idem_key, order_id)
            VALUES(@k, @o)", conn, tx);
        insertIdem.Parameters.AddWithValue("k", idemKey);
        insertIdem.Parameters.AddWithValue("o", orderId);
        await insertIdem.ExecuteNonQueryAsync(ct);

        await tx.CommitAsync(ct);

        return Results.Created($"/orders/{orderId}", new { orderId, status = "CREATED", deduplicated = false });
    }

    [HttpGet("orders/{id:long}")]
    public async Task<IResult> GetById([FromServices] NpgsqlDataSource dataSource, long id, CancellationToken ct)
    {
        await using var conn = await dataSource.OpenConnectionAsync(ct);

        var cmd = new NpgsqlCommand(@"
            SELECT id, customer_id, sku, quantity, unit_price, total_amount, status, created_at
            FROM orders WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct))
        {
            return Results.NotFound();
        }

        return Results.Ok(new
        {
            id = reader.GetInt64(0),
            customerId = reader.GetString(1),
            sku = reader.GetString(2),
            quantity = reader.GetInt32(3),
            unitPrice = reader.GetDecimal(4),
            totalAmount = reader.GetDecimal(5),
            status = reader.GetString(6),
            createdAt = reader.GetDateTime(7)
        });
    }
}
