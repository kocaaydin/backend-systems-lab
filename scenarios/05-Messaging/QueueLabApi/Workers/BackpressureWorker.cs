public sealed class BackpressureWorker : BackgroundService
{
    private readonly QueueLabState _state;

    public BackpressureWorker(QueueLabState state)
    {
        _state = state;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var processedInTick = 0;
            while (processedInTick < 10 && _state.BackpressureQueue.TryDequeue(out _))
            {
                processedInTick++;
                Interlocked.Increment(ref _state.ProcessedBackpressure);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
