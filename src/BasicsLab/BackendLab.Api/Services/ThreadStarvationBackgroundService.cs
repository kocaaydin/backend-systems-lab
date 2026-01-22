using System.Diagnostics;

namespace BackendLab.Api.Services;

/// <summary>
/// Background Worker demonstrating ThreadPool Starvation (Sync-over-Async Antipattern).
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
        _logger.LogInformation("🚀 Thread Starvation Service Starting...");
        
        // Small delay to let application finish startup
        await Task.Delay(2000, stoppingToken);

        try
        {
            await RunExperimentAsync(stoppingToken);
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Experiment Failed");
        }
    }

    private async Task RunExperimentAsync(CancellationToken stoppingToken)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        cts.CancelAfter(TimeSpan.FromSeconds(TimeoutSeconds + 5));
        
        using var activity = ExperimentActivitySource.StartActivity("ThreadStarvationExperiment");
        activity?.SetTag("experiment.name", "Thread Starvation #2.1");
        
        _logger.LogInformation($"🔬 STARTING Experiment: {TotalWorkers} workers, MaxConcurrent={MaxConcurrentWorkers}");

        // Save original limits
        ThreadPool.GetMinThreads(out int origMinW, out int origMinCP);
        ThreadPool.GetMaxThreads(out int origMaxW, out int origMaxCP);

        // Limit ThreadPool to force starvation easier
        int demoMaxThreads = MaxConcurrentWorkers + 5; 
        ThreadPool.SetMaxThreads(demoMaxThreads, demoMaxThreads);
        ThreadPool.SetMinThreads(demoMaxThreads, demoMaxThreads);
        _logger.LogWarning($"⚠️  ThreadPool artificially capped at {demoMaxThreads} threads.");

        var sw = Stopwatch.StartNew();
        using var semaphore = new SemaphoreSlim(MaxConcurrentWorkers, MaxConcurrentWorkers);
        var monitoringCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        
        //Start monitoring thread
        new Thread(() => MonitorThreadPool(monitoringCts.Token)) { IsBackground = true, Name = "Monitor" }.Start();

        var completedWorkers = 0;
        var failedWorkers = 0;

        try
        {
            for (int i = 0; i < TotalWorkers; i++)
            {
                int id = i;
                _ = Task.Run(() => 
                {
                    try
                    {
                        RunWorker(id, semaphore);
                        Interlocked.Increment(ref completedWorkers);
                    }
                    catch 
                    {
                        Interlocked.Increment(ref failedWorkers);
                    }
                });
            }

            _logger.LogInformation("✅ All tasks queued. Waiting for completion...");

            // Wait loop
            while (completedWorkers + failedWorkers < TotalWorkers && !stoppingToken.IsCancellationRequested)
            {
                if (cts.IsCancellationRequested) break;
                await Task.Delay(500, stoppingToken);
            }

            int unfinished = TotalWorkers - (completedWorkers + failedWorkers);
            if (unfinished > 0)
            {
                _logger.LogWarning($"⚠️  WARNING: {unfinished} workers are still RUNNING or PENDING at loop exit.");
                _logger.LogWarning("   -> This demonstrates that if the application stops here (or we stop waiting),");
                _logger.LogWarning("      these operations are cut short/orphaned.");
            }

            sw.Stop();
            bool success = completedWorkers == TotalWorkers;
            
            _logger.LogInformation("--------------------------------------------------");
            if (success)
                _logger.LogInformation($"✅ SUCCESS: All {completedWorkers} workers completed in {sw.ElapsedMilliseconds}ms.");
            else
                _logger.LogError($"❌ FAILURE: Timeout/Starvation detected. Completed: {completedWorkers}/{TotalWorkers}");
            _logger.LogInformation("--------------------------------------------------");



            activity?.SetTag("experiment.success", success);
        }
        finally
        {
            monitoringCts.Cancel();
            ThreadPool.SetMinThreads(origMinW, origMinCP);
            ThreadPool.SetMaxThreads(origMaxW, origMaxCP);
            _logger.LogInformation("🔄 Original ThreadPool limits restored.");
        }
    }

    private void RunWorker(int id, SemaphoreSlim semaphore)
    {
        try
        {
            if (!semaphore.Wait(TimeSpan.FromSeconds(5))) return;

            try
            {
                // THE ANTI-PATTERN: Blocking wait on Async task
                Task.Run(async () => await Task.Delay(WorkerDurationMs)).Wait(); 

                _logger.LogInformation($"✅ Worker {id} done"); // Reduced verbosity
            }
            finally
            {
                semaphore.Release();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Worker {id} error: {ex.Message}");
            throw;
        }
    }

    private void MonitorThreadPool(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                ThreadPool.GetAvailableThreads(out int availableWorkers, out _);
                ThreadPool.GetMaxThreads(out int maxWorkers, out _);

                int usedWorkers = maxWorkers - availableWorkers;

                if (availableWorkers == 0)
                {
                    _logger.LogError($"❌ THREADPOOL STARVATION: {usedWorkers}/{maxWorkers} in use, 0 available!");
                }
                else
                {
                    _logger.LogInformation($"📊 ThreadPool: {usedWorkers}/{maxWorkers} in use, {availableWorkers} available");
                }

                Thread.Sleep(MonitoringIntervalMs);
            }
            catch
            {
                break;
            }
        }
    }
}
