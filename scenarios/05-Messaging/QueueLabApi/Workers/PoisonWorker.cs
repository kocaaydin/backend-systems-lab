public sealed class PoisonWorker : BackgroundService
{
    private readonly QueueLabState _state;

    public PoisonWorker(QueueLabState state)
    {
        _state = state;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_state.PoisonQueue.TryDequeue(out var msg))
            {
                if (msg.Contains("POISON", StringComparison.OrdinalIgnoreCase))
                {
                    Interlocked.Increment(ref _state.PoisonFailures);
                    _state.Dlq.Enqueue(msg);
                }
                else
                {
                    Interlocked.Increment(ref _state.ProcessedPoison);
                }
            }

            await Task.Delay(40, stoppingToken);
        }
    }
}
