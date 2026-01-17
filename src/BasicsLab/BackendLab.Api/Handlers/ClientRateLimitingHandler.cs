namespace BackendLab.Api.Handlers;

public class ClientRateLimitingHandler : DelegatingHandler
{
    private readonly int _limit;
    private int _requestCount = 0;
    private DateTime _lastResetTime = DateTime.UtcNow;
    private readonly object _lock = new object();

    public ClientRateLimitingHandler(int limit)
    {
        _limit = limit;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastResetTime).TotalSeconds >= 1)
            {
                _requestCount = 0;
                _lastResetTime = now;
            }

            if (_requestCount >= _limit)
            {
                // Return 429 Too Many Requests
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.TooManyRequests));
            }

            _requestCount++;
        }

        return base.SendAsync(request, cancellationToken);
    }
}
