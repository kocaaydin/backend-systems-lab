using System.Diagnostics;

namespace BackendLab.Api.Services;

/// <summary>
/// Background Worker for Thread Starvation Experiment (Deney #2.1)
/// Runs automatically when the application starts
/// Demonstrates ThreadPool starvation with Thread + Task.Run + .Wait() pattern
/// </summary>
public class ThreadStarvationBackgroundService : BackgroundService
{
    private readonly ILogger<ThreadStarvationBackgroundService> _logger;
    private static readonly ActivitySource ExperimentActivitySource = new("BackendLab.ThreadStarvation");
    
    // Configuration
    private const int MaxConcurrentWorkers = 50;
    private const int TotalWorkers = 100;
    private const int WorkerDurationMs = 5000;
    private const int TimeoutSeconds = 30;
    private const int MonitoringIntervalMs = 2000;

    public ThreadStarvationBackgroundService(ILogger<ThreadStarvationBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🚀 Thread Starvation Background Service Starting...");
        
        // Small delay to let application finish startup
        await Task.Delay(2000, stoppingToken);

        try
        {
            await RunThreadStarvationExperimentAsync(stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("⏹️  Thread Starvation Experiment Cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Thread Starvation Experiment Failed");
        }
    }

    private async Task RunThreadStarvationExperimentAsync(CancellationToken stoppingToken)
    {
        using var activity = ExperimentActivitySource.StartActivity("ThreadStarvationExperiment");
        activity?.SetTag("experiment.name", "Thread Starvation - Deney #2.1");
        activity?.SetTag("experiment.workers.total", TotalWorkers);
        activity?.SetTag("experiment.workers.max_concurrent", MaxConcurrentWorkers);
        
        _logger.LogInformation("\n╔════════════════════════════════════════════════════════════════════╗");
        _logger.LogInformation("║        Thread Starvation Lab - Deney #2.1 (Background Worker)       ║");
        _logger.LogInformation("║     Demonstrating ThreadPool Starvation with Task.Run + .Wait()    ║");
        _logger.LogInformation("╚════════════════════════════════════════════════════════════════════╝\n");
        
        _logger.LogInformation("Configuration:");
        _logger.LogInformation("  - Total Workers: {TotalWorkers}", TotalWorkers);
        _logger.LogInformation("  - Max Concurrent: {MaxConcurrentWorkers}", MaxConcurrentWorkers);
        _logger.LogInformation("  - Worker Duration: {WorkerDurationMs}ms", WorkerDurationMs);
        _logger.LogInformation("  - Timeout: {TimeoutSeconds}s\n", TimeoutSeconds);

        var sw = Stopwatch.StartNew();
        var semaphore = new SemaphoreSlim(MaxConcurrentWorkers, MaxConcurrentWorkers);
        var monitoringCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        
        // Start monitoring task
        var monitoringTask = MonitorThreadPoolAsync(_logger, monitoringCts.Token);

        _logger.LogInformation("📊 Starting worker initialization...\n");

        var workerTasks = new List<Task>();
        var completedWorkers = 0;
        var failedWorkers = 0;

        try
        {
            // Launch all workers - each on a separate thread
            for (int i = 0; i < TotalWorkers; i++)
            {
                int workerId = i;
                
                // ❌ PROBLEMATIC PATTERN: Create thread, then Task.Run + .Wait() inside
                var thread = new Thread(() =>
                {
                    try
                    {
                        RunWorkerSync(workerId, semaphore);
                        Interlocked.Increment(ref completedWorkers);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Worker {WorkerId} failed", workerId);
                        Interlocked.Increment(ref failedWorkers);
                    }
                })
                {
                    IsBackground = false,
                    Name = $"Worker-{i}"
                };

                thread.Start();
            }

            _logger.LogInformation("✅ All {Count} worker threads launched\n", TotalWorkers);

            // Wait for completion or timeout
            bool completed = false;
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(TimeoutSeconds), stoppingToken);
            
            // Poll for completion or timeout
            while (!completed && !stoppingToken.IsCancellationRequested)
            {
                int current = Interlocked.CompareExchange(ref completedWorkers, 0, 0);
                
                if (current + failedWorkers >= TotalWorkers)
                {
                    completed = true;
                    break;
                }

                if (timeoutTask.IsCompleted)
                {
                    break;
                }

                await Task.Delay(500, stoppingToken);
            }

            sw.Stop();

            _logger.LogInformation("\n╔════════════════════════════════════════════════════════════════════╗");
            if (completed)
            {
                _logger.LogInformation("║ ✅ All workers completed successfully                              ║");
                activity?.SetTag("experiment.result", "success");
                activity?.SetTag("experiment.starved", false);
            }
            else
            {
                _logger.LogWarning("║ ⚠️  TIMEOUT! Workers did not complete within {TimeoutSeconds}s         ║", TimeoutSeconds);
                _logger.LogWarning("║ ❌ ThreadPool is DEADLOCKED - threads blocked by Task.Run + .Wait() ║");
                activity?.SetTag("experiment.result", "starvation_detected");
                activity?.SetTag("experiment.starved", true);
            }
            _logger.LogInformation("╚════════════════════════════════════════════════════════════════════╝\n");

            _logger.LogInformation("📈 Final Statistics:");
            _logger.LogInformation("  - Total Elapsed: {ElapsedMs}ms", sw.ElapsedMilliseconds);
            _logger.LogInformation("  - Completed Workers: {Completed}/{Total}", completedWorkers, TotalWorkers);
            _logger.LogInformation("  - Failed Workers: {Failed}/{Total}\n", failedWorkers, TotalWorkers);
            
            activity?.SetTag("experiment.elapsed_ms", sw.ElapsedMilliseconds);
            activity?.SetTag("experiment.workers.completed", completedWorkers);
            activity?.SetTag("experiment.workers.failed", failedWorkers);
        }
        finally
        {
            monitoringCts.Cancel();
            semaphore.Dispose();
            monitoringCts.Dispose();
        }

        _logger.LogInformation("🏁 Thread Starvation Experiment Completed\n");
    }

    private void RunWorkerSync(int workerId, SemaphoreSlim semaphore)
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        var threadName = Thread.CurrentThread.Name;

        _logger.LogDebug("👷 Worker {WorkerId} ({ThreadName}) started on Thread {ThreadId}", 
            workerId, threadName, threadId);

        try
        {
            // Acquire semaphore slot (may block if limit reached)
            if (!semaphore.Wait(TimeSpan.FromSeconds(5)))
            {
                _logger.LogWarning("⏱️  Worker {WorkerId} timeout waiting for semaphore", workerId);
                return;
            }

            _logger.LogDebug("🟢 Worker {WorkerId} acquired semaphore slot on Thread {ThreadId}", 
                workerId, threadId);

            try
            {
                // ❌ PROBLEMATIC PATTERN: Task.Run + synchronous .Wait() 
                // This thread (from ThreadPool) is now blocked, preventing ThreadPool from
                // processing other queued tasks
                var delayTask = Task.Run(async () =>
                {
                    _logger.LogDebug("📥 Worker {WorkerId} Task.Run executed on Thread {ThreadId}",
                        workerId, Thread.CurrentThread.ManagedThreadId);
                    await Task.Delay(WorkerDurationMs);
                });

                // ❌ DEADLOCK RISK: Synchronous .Wait() blocks this thread!
                delayTask.Wait(TimeSpan.FromSeconds(10));

                _logger.LogInformation("✅ Worker {WorkerId} completed on Thread {ThreadId}", 
                    workerId, threadId);
            }
            finally
            {
                semaphore.Release();
                _logger.LogDebug("🔴 Worker {WorkerId} released semaphore slot", workerId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Worker {WorkerId} error on Thread {ThreadId}", workerId, threadId);
        }
    }

    private async Task MonitorThreadPoolAsync(ILogger logger, CancellationToken ct)
    {
        logger.LogInformation("🔍 ThreadPool Monitoring Started\n");

        try
        {
            while (!ct.IsCancellationRequested)
            {
                ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
                ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);

                var utilizationPercent = ((maxWorkerThreads - workerThreads) * 100) / maxWorkerThreads;

                logger.LogInformation("📊 [ThreadPool] Available: {Available}/{Max} (Utilization: {Percent}%)",
                    workerThreads, maxWorkerThreads, utilizationPercent);

                if (utilizationPercent > 90)
                    logger.LogWarning("   ⚠️  HIGH UTILIZATION ({Percent}%) - Potential starvation!", utilizationPercent);

                if (workerThreads == 0)
                    logger.LogError("   ❌ NO AVAILABLE THREADS - COMPLETE STARVATION!");

                await Task.Delay(MonitoringIntervalMs, ct);
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when cancellation is requested
        }

        logger.LogInformation("\n🔍 ThreadPool Monitoring Stopped");
    }
}
