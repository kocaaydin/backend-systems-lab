public sealed class TcpLabWorker : BackgroundService
{
    private readonly QueueLabState _state;

    public TcpLabWorker(QueueLabState state)
    {
        _state = state;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tick = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            tick++;

            var kafkaDrain = 0;
            while (kafkaDrain < 25 && _state.KafkaBacklog.TryDequeue(out _))
            {
                kafkaDrain++;
            }

            var rabbitDrain = 0;
            while (rabbitDrain < 80 && _state.RabbitInMemoryBuffer.TryDequeue(out _))
            {
                rabbitDrain++;
            }

            if (tick % 20 == 0)
            {
                Interlocked.Increment(ref _state.RebalancePauses);
                await Task.Delay(700, stoppingToken);
            }

            await Task.Delay(250, stoppingToken);
        }
    }
}
