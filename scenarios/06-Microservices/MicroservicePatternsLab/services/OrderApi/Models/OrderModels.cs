public sealed record OrderCreateRequest(string CustomerId, string Sku, int Quantity, decimal UnitPrice);
public sealed record OrderCreatedEvent(long OrderId, string CustomerId, string Sku, int Quantity, decimal UnitPrice, decimal TotalAmount, DateTime CreatedAt);
