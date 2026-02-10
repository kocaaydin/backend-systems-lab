using System.Collections.Concurrent;

public sealed class QueueLabState
{
    private long _id;

    public ConcurrentQueue<string> BackpressureQueue { get; } = new();
    public ConcurrentQueue<string> PoisonQueue { get; } = new();
    public ConcurrentQueue<string> Dlq { get; } = new();
    public ConcurrentQueue<string> KafkaBacklog { get; } = new();
    public ConcurrentQueue<string> RabbitInMemoryBuffer { get; } = new();
    public ConcurrentQueue<int> ConnectionChurnSignals { get; } = new();
    public ConcurrentQueue<HolJob> HolQueue { get; } = new();

    public int BackpressureQueueCount => BackpressureQueue.Count;
    public int PoisonQueueCount => PoisonQueue.Count;
    public int DlqCount => Dlq.Count;
    public int KafkaBacklogCount => KafkaBacklog.Count;
    public int RabbitInMemoryBufferCount => RabbitInMemoryBuffer.Count;
    public int HolQueueCount => HolQueue.Count;

    public long ProcessedBackpressure;
    public long ProcessedPoison;
    public long PoisonFailures;
    public long RebalancePauses;

    public long NextId() => Interlocked.Increment(ref _id);
}
