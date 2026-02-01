using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    #region Configuration
    // Simulation Parameters
    private const int ProducerDelayMs = 100; // Produces every 0.1s
    private const int ConsumerDelayMs = 500; // Consumes every 0.5s (5x slower)
    private const int MaxQueueSize = 20;
    #endregion

    #region Main Execution
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Backpressure Simulation ===");
        Console.WriteLine($"Producer Speed: 1 item/{ProducerDelayMs}ms");
        Console.WriteLine($"Consumer Speed: 1 item/{ConsumerDelayMs}ms");
        Console.WriteLine($"Critical Queue Size: {MaxQueueSize}");
        Console.WriteLine("------------------------------------------");

        var queue = new ConcurrentQueue<int>();
        var cts = new CancellationTokenSource();

        var producer = Task.Run(() => RunProducer(queue, cts.Token));
        var consumer = Task.Run(() => RunConsumer(queue, cts.Token));
        var monitor = Task.Run(() => RunMonitor(queue, cts.Token));

        // Run for 15 seconds then stop
        await Task.Delay(15000);
        cts.Cancel();
        
        Console.WriteLine("\n------------------------------------------");
        Console.WriteLine("Simulation Stopped.");
    }
    #endregion

    #region Actor Logic
    static async Task RunProducer(ConcurrentQueue<int> queue, CancellationToken token)
    {
        int id = 1;
        while (!token.IsCancellationRequested)
        {
            queue.Enqueue(id);
            // Console.WriteLine($"[Producer] Produced Item #{id}");
            id++;
            await Task.Delay(ProducerDelayMs, token);
        }
    }

    static async Task RunConsumer(ConcurrentQueue<int> queue, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (queue.TryDequeue(out int item))
            {
                // Console.WriteLine($"[Consumer] Processed Item #{item}");
            }
            await Task.Delay(ConsumerDelayMs, token);
        }
    }
    #endregion

    #region Monitoring Logic
    static async Task RunMonitor(ConcurrentQueue<int> queue, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            int count = queue.Count;
            Console.Write($"Queue Size: {count} ");

            // Visualization of queue size
            int bars = count / 2;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < bars; i++)
            {
                if (i > 10) Console.ForegroundColor = ConsoleColor.Yellow;
                if (i > 20) Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("|");
            }
            Console.ResetColor();
            Console.WriteLine("]");

            if (count > MaxQueueSize)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("!!! SYSTEM OVERLOAD WARNING: CONSUMER CANNOT KEEP UP !!!");
                Console.WriteLine("   -> Latency is increasing.");
                Console.WriteLine("   -> Memory usage is growing.");
                Console.ResetColor();
            }

            await Task.Delay(1000, token);
        }
    }
    #endregion
}
