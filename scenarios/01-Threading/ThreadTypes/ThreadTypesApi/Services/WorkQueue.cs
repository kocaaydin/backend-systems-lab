using System.Threading.Channels;

namespace ThreadTypesApi.Services;

public sealed record QueuedWork(int WorkMs, DateTime EnqueuedAtUtc);

public sealed record QueueSnapshot(long Enqueued, long Processed, int Queued, int ActiveWorkers);

public sealed class WorkQueue
{
    private readonly Channel<QueuedWork> _channel = Channel.CreateUnbounded<QueuedWork>();
    private long _enqueued;
    private long _processed;
    private int _queued;
    private int _activeWorkers;

    public ValueTask EnqueueAsync(QueuedWork work, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref _enqueued);
        Interlocked.Increment(ref _queued);
        return _channel.Writer.WriteAsync(work, cancellationToken);
    }

    public IAsyncEnumerable<QueuedWork> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }

    public void MarkWorkerStart()
    {
        Interlocked.Decrement(ref _queued);
        Interlocked.Increment(ref _activeWorkers);
    }

    public void MarkWorkerDone()
    {
        Interlocked.Increment(ref _processed);
        Interlocked.Decrement(ref _activeWorkers);
    }

    public QueueSnapshot Snapshot()
    {
        return new QueueSnapshot(
            Enqueued: Interlocked.Read(ref _enqueued),
            Processed: Interlocked.Read(ref _processed),
            Queued: Volatile.Read(ref _queued),
            ActiveWorkers: Volatile.Read(ref _activeWorkers));
    }
}
