using System.Diagnostics;

namespace BackendLab.Api.Services;

/// <summary>
/// Thread Starvation Experiment Service
/// Demonstrates ThreadPool starvation with Task.Run + .Wait() pattern on ThreadPool threads
/// </summary>
public class ThreadStarvationService
{
    private readonly ILogger<ThreadStarvationService> _logger;
    
    public const int MaxConcurrentWorkers = 50;
    public const int TotalWorkers = 100;
    public const int WorkerDurationMs = 5000;
    public const int TimeoutSeconds = 30;

    public ThreadStarvationService(ILogger<ThreadStarvationService> logger)
    {
        _logger = logger;
    }

    public async Task<ThreadStarvationResult> RunExperimentAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("╔════════════════════════════════════════════════════════════════════╗");
        _logger.LogInformation("║        Thread Starvation Lab - Deney #2.1                          ║");
        _logger.LogInformation("║     Demonstrating ThreadPool Starvation with Task.Run + .Wait()    ║");
        _logger.LogInformation("╚════════════════════════════════════════════════════════════════════╝");
        _logger.LogInformation("Configuration:");
        _logger.LogInformation("  - Total Workers: {TotalWorkers}", TotalWorkers);
        _logger.LogInformation("  - Max Concurrent: {MaxConcurrentWorkers}", MaxConcurrentWorkers);
        _logger.LogInformation("  - Worker Duration: {WorkerDurationMs}ms", WorkerDurationMs);

        var result = new ThreadStarvationResult();
        var sw = Stopwatch.StartNew();
        var semaphore = new SemaphoreSlim(MaxConcurrentWorkers, MaxConcurrentWorkers);
        var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var monitoringTask = MonitorThreadPoolAsync(_logger, cts.Token);

        _logger.LogInformation("\n📊 Starting worker initialization...\n");

        var workerTasks = new List<Task>();

        try
        {
            // Launch all workers
            for (int i = 0; i < TotalWorkers; i++)
            {
                int workerId = i;
                var task = Task.Run(() => RunWorkerAsync(workerId, semaphore, _logger), cts.Token);
                workerTasks.Add(task);
            }

            _logger.LogInformation("✅ All {Count} workers launched", TotalWorkers);

            // Wait for all workers or timeout
            var completedTask = await Task.WhenAny(
                Task.WhenAll(workerTasks),
                Task.Delay(TimeSpan.FromSeconds(TimeoutSeconds), cts.Token)
            );

            sw.Stop();

            if (completedTask == Task.WhenAll(workerTasks))
            {
                _logger.LogInformation("✅ All workers completed successfully in {ElapsedMs}ms", sw.ElapsedMilliseconds);
                result.IsStarved = false;
                result.Status = "Completed";
            }
            else
            {
                _logger.LogWarning("⚠️  TIMEOUT! Workers did not complete within {TimeoutSeconds} seconds (likely STARVATION)", TimeoutSeconds);
                _logger.LogWarning("❌ ThreadPool is DEADLOCKED - workers waiting for completion while blocking threads");
                result.IsStarved = true;
                result.Status = "Starvation Detected";
            }

            result.CompletedWorkers = workerTasks.Count(t => t.IsCompleted);
            result.TotalWorkers = TotalWorkers;
            result.ElapsedMs = sw.ElapsedMilliseconds;
        }
        finally
        {
            cts.Cancel();
            await Task.Delay(500);
            semaphore.Dispose();
            cts.Dispose();
        }

        _logger.LogInformation("\n📈 Final Statistics:");
        _logger.LogInformation("  - Total Elapsed: {ElapsedMs}ms", result.ElapsedMs);
        _logger.LogInformation("  - Completed Workers: {Completed}/{Total}", result.CompletedWorkers, TotalWorkers);

        return result;
    }

    private async Task RunWorkerAsync(int workerId, SemaphoreSlim semaphore, ILogger logger)
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;

        logger.LogDebug("👷 Worker {WorkerId} started on Thread {ThreadId}", workerId, threadId);

        try
        {
            await semaphore.WaitAsync();
            logger.LogDebug("🟢 Worker {WorkerId} acquired semaphore slot on Thread {ThreadId}", workerId, threadId);

            try
            {
                // ❌ PROBLEMATIC PATTERN: Task.Run + .Wait() on ThreadPool thread
                var delayTask = Task.Run(async () =>
                {
                    logger.LogDebug("📥 Worker {WorkerId} Task.Run executed on Thread {ThreadId}",
                        workerId, Thread.CurrentThread.ManagedThreadId);
                    await Task.Delay(WorkerDurationMs);
                });

                // ❌ DEADLOCK RISK: Synchronously wait on ThreadPool thread!
                delayTask.Wait();

                logger.LogInformation("✅ Worker {WorkerId} completed on Thread {ThreadId}", workerId, threadId);
            }
            finally
            {
                semaphore.Release();
                logger.LogDebug("🔴 Worker {WorkerId} released semaphore slot", workerId);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Worker {WorkerId} failed on Thread {ThreadId}", workerId, threadId);
        }
    }

    private async Task MonitorThreadPoolAsync(ILogger logger, CancellationToken ct)
    {
        logger.LogInformation("🔍 ThreadPool Monitoring Started (every 2 seconds)...\n");

        while (!ct.IsCancellationRequested)
        {
            try
            {
                ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
                ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);

                var utilizationPercent = ((maxWorkerThreads - workerThreads) * 100) / maxWorkerThreads;

                logger.LogInformation("📊 ThreadPool Stats:");
                logger.LogInformation("   Worker Threads: {Available}/{Max} (Utilization: {Percent}%)",
                    workerThreads, maxWorkerThreads, utilizationPercent);
                logger.LogInformation("   Completion Port Threads: {Available}/{Max}",
                    completionPortThreads, maxCompletionPortThreads);

                if (utilizationPercent > 90)
                    logger.LogWarning("   ⚠️  HIGH UTILIZATION - Potential starvation!");

                if (workerThreads == 0)
                    logger.LogError("   ❌ NO AVAILABLE THREADS - COMPLETE STARVATION!");

                await Task.Delay(2000, ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        logger.LogInformation("\n🔍 ThreadPool Monitoring Stopped\n");
    }
}

public class ThreadStarvationResult
{
    public bool IsStarved { get; set; }
    public string Status { get; set; } = "";
    public int CompletedWorkers { get; set; }
    public int TotalWorkers { get; set; }
    public long ElapsedMs { get; set; }
}
