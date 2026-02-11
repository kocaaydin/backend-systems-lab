namespace ThreadTypesApi.Services;

public sealed class FinalizerProbe
{
    private static long _created;
    private static long _finalized;

    public FinalizerProbe()
    {
        Interlocked.Increment(ref _created);
    }

    ~FinalizerProbe()
    {
        Interlocked.Increment(ref _finalized);
    }

    public static (long Created, long Finalized) Snapshot()
    {
        return (Interlocked.Read(ref _created), Interlocked.Read(ref _finalized));
    }
}
