namespace ThreadTypesApi.Services;

public sealed class QueueWorkerService(WorkQueue queue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var work in queue.ReadAllAsync(stoppingToken))
        {
            queue.MarkWorkerStart();
            try
            {
                await Task.Delay(work.WorkMs, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Host shutdown path.
            }
            finally
            {
                queue.MarkWorkerDone();
            }
        }
    }
}
