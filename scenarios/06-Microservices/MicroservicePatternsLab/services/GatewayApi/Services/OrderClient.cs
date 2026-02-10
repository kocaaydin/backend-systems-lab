using System.Text;

public interface IOrderClient
{
    Task<IResult> CreateOrderAsync(string idempotencyKey, OrderCreateRequest request, CancellationToken ct);
    Task<IResult> GetOrderAsync(long id, CancellationToken ct);
}

public sealed class OrderClient : IOrderClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OrderClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IResult> CreateOrderAsync(string idempotencyKey, OrderCreateRequest request, CancellationToken ct)
    {
        var client = _httpClientFactory.CreateClient("orders");
        using var msg = new HttpRequestMessage(HttpMethod.Post, "/orders")
        {
            Content = JsonContent.Create(request)
        };

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            msg.Headers.TryAddWithoutValidation("Idempotency-Key", idempotencyKey);
        }

        using var res = await client.SendAsync(msg, ct);
        var body = await res.Content.ReadAsStringAsync(ct);
        return Results.Content(body, "application/json", Encoding.UTF8, (int)res.StatusCode);
    }

    public async Task<IResult> GetOrderAsync(long id, CancellationToken ct)
    {
        var client = _httpClientFactory.CreateClient("orders");
        using var res = await client.GetAsync($"/orders/{id}", ct);
        var body = await res.Content.ReadAsStringAsync(ct);
        return Results.Content(body, "application/json", Encoding.UTF8, (int)res.StatusCode);
    }
}
