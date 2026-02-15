namespace ThreadTypesApi.Services.Gc;

public class FinalizableObject
{
    // Static counters to track global state
    public static long AllocatedCount = 0;
    public static long FinalizedCount = 0;

    // Payload to occupy some memory (approx 1KB)
    private readonly byte[] _payload = new byte[1024];

    public FinalizableObject()
    {
        Interlocked.Increment(ref AllocatedCount);
    }

    ~FinalizableObject()
    {
        // This runs on the Finalizer Thread
        Interlocked.Increment(ref FinalizedCount);
        // Simulate some work in finalizer to make it observable
        Thread.SpinWait(100); 
    }
}
