public sealed class HolWorker(QueueLabState state) : BackgroundService
{
    private readonly QueueLabState _state = state;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_state.HolQueue.TryDequeue(out var job))
            {
                try
                {
                    await Task.Delay(job.DurationMs, stoppingToken);
                    job.Completion.TrySetResult(true);
                }
                catch (OperationCanceledException)
                {
                    job.Completion.TrySetCanceled(stoppingToken);
                }
            }
            else
            {
                await Task.Delay(10, stoppingToken);
            }
        }
    }
}
