using System.Threading.Channels;

namespace QueueLab.Api.Services;

public record HolJob(int Id, int DurationMs);

public class HolChannel
{
    private readonly Channel<HolJob> _channel;

    public HolChannel()
    {
        _channel = Channel.CreateUnbounded<HolJob>();
    }

    public ChannelWriter<HolJob> Writer => _channel.Writer;
    public ChannelReader<HolJob> Reader => _channel.Reader;
}

public class HolWorker : BackgroundService
{
    private readonly HolChannel _channel;
    private readonly ILogger<HolWorker> _logger;

    public HolWorker(HolChannel channel, ILogger<HolWorker> logger)
    {
        _channel = channel;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("HOL Worker Started. Single Thread Processing.");
        
        await foreach (var job in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            // Simulate processing time
            if (job.DurationMs > 1000)
            {
                _logger.LogWarning($"[HOL] Processing SLOW job {job.Id} ({job.DurationMs}ms)... Blocking queue.");
            }
            
            await Task.Delay(job.DurationMs, stoppingToken);
            
            if (job.DurationMs > 1000)
            {
                _logger.LogInformation($"[HOL] Finished SLOW job {job.Id}.");
            }
        }
    }
}
