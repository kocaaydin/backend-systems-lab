using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public sealed class GatewayController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { service = "gateway", status = "ok" });
    }

    [HttpPost("api/orders")]
    public async Task<IResult> CreateOrder([FromServices] IOrderClient client, [FromBody] OrderCreateRequest request, CancellationToken ct)
    {
        var idemKey = Request.Headers["Idempotency-Key"].ToString();
        return await client.CreateOrderAsync(idemKey, request, ct);
    }

    [HttpGet("api/orders/{id:long}")]
    public async Task<IResult> GetOrder([FromServices] IOrderClient client, long id, CancellationToken ct)
    {
        return await client.GetOrderAsync(id, ct);
    }
}
