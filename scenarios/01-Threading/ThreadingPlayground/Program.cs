using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var mode = args.FirstOrDefault()?.ToLowerInvariant();

        if (mode == "longrunning")
        {
            await RunLongRunningComparison();
            return;
        }

        if (mode == "consumer-loss")
        {
            await ConsumerLossWorker.RunAsync(args.Skip(1).ToArray());
            return;
        }

        if (string.IsNullOrEmpty(mode))
            return;

        PrintUsage();
    }

    static async Task RunLongRunningComparison()
    {
        const int jobCount = 200;
        const int sleepMs = 200;

        Console.WriteLine("Task.Run vs LongRunning (Dongulu Ornek)\n");
        Console.WriteLine($"Toplam is: {jobCount}, her is: Thread.Sleep({sleepMs})");
        Console.WriteLine("Not: Bu test bloklayan isi simule eder.\n");

        var taskRun = await RunBatch(jobCount, sleepMs, useLongRunning: false);
        var longRunning = await RunBatch(jobCount, sleepMs, useLongRunning: true);

        Console.WriteLine("Sonuc:");
        Console.WriteLine($"Task.Run     -> ThreadPool: {taskRun.PoolCount}, Dedicated: {taskRun.DedicatedCount}, Toplam Sure: {taskRun.ElapsedMs:F0} ms");
        Console.WriteLine($"LongRunning  -> ThreadPool: {longRunning.PoolCount}, Dedicated: {longRunning.DedicatedCount}, Toplam Sure: {longRunning.ElapsedMs:F0} ms");
    }

    static async Task<BatchResult> RunBatch(int jobCount, int sleepMs, bool useLongRunning)
    {
        var tasks = new Task<bool>[jobCount];
        var sw = Stopwatch.StartNew();

        for (int i = 0; i < jobCount; i++)
        {
            tasks[i] = useLongRunning
                ? Task.Factory.StartNew(() =>
                {
                    bool isPool = Thread.CurrentThread.IsThreadPoolThread;
                    Thread.Sleep(sleepMs);
                    return isPool;
                }, TaskCreationOptions.LongRunning)
                : Task.Run(() =>
                {
                    bool isPool = Thread.CurrentThread.IsThreadPoolThread;
                    Thread.Sleep(sleepMs);
                    return isPool;
                });
        }

        var types = await Task.WhenAll(tasks);
        sw.Stop();

        int pool = types.Count(x => x);
        return new BatchResult(pool, jobCount - pool, sw.Elapsed.TotalMilliseconds);
    }

    static void PrintUsage()
    {
        Console.WriteLine("Kullanim:");
        Console.WriteLine("  dotnet run --project scenarios/01-Threading/ThreadingPlayground/ThreadingPlayground.csproj -- longrunning");
        Console.WriteLine("  dotnet run --project scenarios/01-Threading/ThreadingPlayground/ThreadingPlayground.csproj -- consumer-loss --scheduler taskrun");
        Console.WriteLine();
        Console.WriteLine("consumer-loss opsiyonlari:");
        Console.WriteLine("  --scheduler taskrun|longrunning");
        Console.WriteLine("  --iterations 10000");
        Console.WriteLine("  --work-ms 250");
        Console.WriteLine("  --linger-ms 1500");
        Console.WriteLine("  --results-dir <path>");
    }

    record BatchResult(int PoolCount, int DedicatedCount, double ElapsedMs);
}
