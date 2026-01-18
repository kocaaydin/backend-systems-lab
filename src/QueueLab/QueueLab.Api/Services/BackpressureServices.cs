using System.Threading.Channels;

namespace QueueLab.Api.Services;

public class BackpressureChannel
{
    private readonly Channel<int> _channel;

    public BackpressureChannel()
    {
        // Unbounded to demonstrate "What happens if we don't apply backpressure" (Memory leak/Crash)
        _channel = Channel.CreateUnbounded<int>();
    }

    public ChannelWriter<int> Writer => _channel.Writer;
    public ChannelReader<int> Reader => _channel.Reader;
    
    public int Count => _channel.Reader.Count;
}

public class BackpressureWorker : BackgroundService
{
    private readonly BackpressureChannel _channel;
    private readonly ILogger<BackpressureWorker> _logger;

    public BackpressureWorker(BackpressureChannel channel, ILogger<BackpressureWorker> logger)
    {
        _channel = channel;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Backpressure Consumer Started. Max capacity: 10/sec");
        
        // Consumer can process max 10 items per second (simulated)
        // If producer sends 50k, this will lag behind massiveley.
        var limit = TimeSpan.FromMilliseconds(100); 

        await foreach (var item in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            // Simulate work
            await Task.Delay(limit, stoppingToken);
            
            if (_channel.Count % 100 == 0)
            {
                _logger.LogInformation($"[BackpressureWorker] Processed item {item}. Queue size: {_channel.Count}");
            }
        }
    }
}
