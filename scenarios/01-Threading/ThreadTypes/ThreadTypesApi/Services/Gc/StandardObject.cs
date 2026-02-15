namespace ThreadTypesApi.Services.Gc;

public class StandardObject
{
    // Payload to occupy some memory (approx 1KB)
    private readonly byte[] _payload = new byte[1024];
}
