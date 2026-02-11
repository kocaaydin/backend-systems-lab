using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThreadTypesApi.Services;

namespace ThreadTypesApi.Controllers;

[ApiController]
[Route("thread-types")]
public sealed class ThreadTypesController : ControllerBase
{
    [HttpGet("info")]
    public IActionResult Info()
    {
        return Ok(new
        {
            managedThreadId = Thread.CurrentThread.ManagedThreadId,
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread,
            processorCount = Environment.ProcessorCount
        });
    }

    [HttpGet("fast")]
    public IActionResult Fast()
    {
        var sw = Stopwatch.StartNew();
        LogStart("fast", null);
        sw.Stop();
        LogEnd("fast", sw.ElapsedMilliseconds, null);
        return Ok(new
        {
            endpoint = "fast",
            elapsedMs = sw.ElapsedMilliseconds,
            managedThreadId = Thread.CurrentThread.ManagedThreadId,
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread
        });
    }

    [HttpGet("io-async")]
    public async Task<IActionResult> IoAsync([FromQuery] int delayMs = 250, CancellationToken cancellationToken = default)
    {
        var safeDelay = Math.Clamp(delayMs, 1, 10000);
        var sw = Stopwatch.StartNew();
        LogStart("io-async", $"delayMs={safeDelay}");

        await Task.Delay(safeDelay, cancellationToken);

        sw.Stop();
        LogEnd("io-async", sw.ElapsedMilliseconds, $"delayMs={safeDelay}");
        return Ok(new
        {
            endpoint = "io-async",
            delayMs = safeDelay,
            elapsedMs = sw.ElapsedMilliseconds,
            managedThreadId = Thread.CurrentThread.ManagedThreadId,
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread
        });
    }

    [HttpGet("cpu-heavy-threadpool")]
    public async Task<IActionResult> CpuHeavyThreadPool([FromServices] CpuCalculator calculator, [FromQuery] int n = 20000)
    {
        var safeN = Math.Clamp(n, 1000, 300000);
        var sw = Stopwatch.StartNew();
        LogStart("cpu-heavy-threadpool", $"n={safeN}");

        var result = await Task.Run(() => calculator.CountPrimes(safeN));

        sw.Stop();
        LogEnd("cpu-heavy-threadpool", sw.ElapsedMilliseconds, $"n={safeN}");
        return Ok(new
        {
            endpoint = "cpu-heavy-threadpool",
            n = safeN,
            primeCount = result,
            elapsedMs = sw.ElapsedMilliseconds,
            managedThreadId = Thread.CurrentThread.ManagedThreadId,
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread
        });
    }

    [HttpGet("cpu-heavy-dedicated")]
    public async Task<IActionResult> CpuHeavyDedicated([FromServices] CpuCalculator calculator, [FromQuery] int n = 20000)
    {
        var safeN = Math.Clamp(n, 1000, 300000);
        var sw = Stopwatch.StartNew();
        LogStart("cpu-heavy-dedicated", $"n={safeN}");

        var tcs = new TaskCompletionSource<(int PrimeCount, int ThreadId, bool IsPool)>(TaskCreationOptions.RunContinuationsAsynchronously);
        var thread = new Thread(() =>
        {
            try
            {
                var count = calculator.CountPrimes(safeN);
                tcs.SetResult((count, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        })
        {
            IsBackground = true,
            Name = "cpu-heavy-dedicated-thread"
        };

        thread.Start();
        var result = await tcs.Task;

        sw.Stop();
        LogEnd("cpu-heavy-dedicated", sw.ElapsedMilliseconds, $"n={safeN}");
        return Ok(new
        {
            endpoint = "cpu-heavy-dedicated",
            n = safeN,
            primeCount = result.PrimeCount,
            elapsedMs = sw.ElapsedMilliseconds,
            requestThreadId = Thread.CurrentThread.ManagedThreadId,
            requestThreadIsPool = Thread.CurrentThread.IsThreadPoolThread,
            workerThreadId = result.ThreadId,
            workerThreadIsPool = result.IsPool
        });
    }

    [HttpGet("starvation/blocking")]
    public IActionResult StarvationBlocking([FromQuery] int blockMs = 3000)
    {
        var safeBlock = Math.Clamp(blockMs, 1, 15000);
        var sw = Stopwatch.StartNew();
        LogStart("starvation/blocking", $"blockMs={safeBlock}");

        Thread.Sleep(safeBlock);

        sw.Stop();
        LogEnd("starvation/blocking", sw.ElapsedMilliseconds, $"blockMs={safeBlock}");
        return Ok(new
        {
            endpoint = "starvation/blocking",
            blockMs = safeBlock,
            elapsedMs = sw.ElapsedMilliseconds,
            managedThreadId = Thread.CurrentThread.ManagedThreadId,
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread
        });
    }

    [HttpGet("cpu-cancellable")]
    public IActionResult CpuCancellable([FromServices] CpuCalculator calculator, [FromQuery] int n = 250000, [FromQuery] int checkEvery = 200, CancellationToken cancellationToken = default)
    {
        var safeN = Math.Clamp(n, 1000, 500000);
        var safeCheckEvery = Math.Clamp(checkEvery, 1, 10000);
        var sw = Stopwatch.StartNew();
        LogStart("cpu-cancellable", $"n={safeN},checkEvery={safeCheckEvery}");

        try
        {
            var count = calculator.CountPrimesCancellable(safeN, cancellationToken, safeCheckEvery);
            sw.Stop();
            LogEnd("cpu-cancellable", sw.ElapsedMilliseconds, $"n={safeN},completed=true");
            return Ok(new
            {
                endpoint = "cpu-cancellable",
                n = safeN,
                checkEvery = safeCheckEvery,
                primeCount = count,
                elapsedMs = sw.ElapsedMilliseconds,
                cancelled = false
            });
        }
        catch (OperationCanceledException)
        {
            sw.Stop();
            LogEnd("cpu-cancellable", sw.ElapsedMilliseconds, $"n={safeN},completed=false");
            return StatusCode(499, new
            {
                endpoint = "cpu-cancellable",
                n = safeN,
                elapsedMs = sw.ElapsedMilliseconds,
                cancelled = true
            });
        }
    }

    [HttpPost("queue/enqueue")]
    public async Task<IActionResult> Enqueue([FromServices] WorkQueue queue, [FromQuery] int items = 10, [FromQuery] int workMs = 300, CancellationToken cancellationToken = default)
    {
        var safeItems = Math.Clamp(items, 1, 5000);
        var safeWorkMs = Math.Clamp(workMs, 1, 10000);
        LogStart("queue/enqueue", $"items={safeItems},workMs={safeWorkMs}");

        for (var i = 0; i < safeItems; i++)
        {
            await queue.EnqueueAsync(new QueuedWork(safeWorkMs, DateTime.UtcNow), cancellationToken);
        }

        var snapshot = queue.Snapshot();
        LogEnd("queue/enqueue", 0, $"queued={snapshot.Queued}");
        return Ok(new
        {
            endpoint = "queue/enqueue",
            items = safeItems,
            workMs = safeWorkMs,
            snapshot
        });
    }

    [HttpGet("queue/status")]
    public IActionResult QueueStatus([FromServices] WorkQueue queue)
    {
        return Ok(new
        {
            endpoint = "queue/status",
            snapshot = queue.Snapshot()
        });
    }

    [HttpPost("finalizer/create")]
    public IActionResult CreateFinalizerSamples([FromQuery] int count = 10000)
    {
        var safeCount = Math.Clamp(count, 1, 2_000_000);
        LogStart("finalizer/create", $"count={safeCount}");

        for (var i = 0; i < safeCount; i++)
        {
            _ = new FinalizerProbe();
        }

        var snapshot = FinalizerProbe.Snapshot();
        LogEnd("finalizer/create", 0, $"created={snapshot.Created},finalized={snapshot.Finalized}");
        return Ok(new
        {
            endpoint = "finalizer/create",
            count = safeCount,
            created = snapshot.Created,
            finalized = snapshot.Finalized
        });
    }

    [HttpPost("finalizer/collect")]
    public IActionResult ForceCollect()
    {
        LogStart("finalizer/collect", null);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var snapshot = FinalizerProbe.Snapshot();
        LogEnd("finalizer/collect", 0, $"created={snapshot.Created},finalized={snapshot.Finalized}");
        return Ok(new
        {
            endpoint = "finalizer/collect",
            created = snapshot.Created,
            finalized = snapshot.Finalized
        });
    }

    [HttpGet("finalizer/stats")]
    public IActionResult FinalizerStats()
    {
        var snapshot = FinalizerProbe.Snapshot();
        return Ok(new
        {
            endpoint = "finalizer/stats",
            created = snapshot.Created,
            finalized = snapshot.Finalized
        });
    }

    private static void LogStart(string endpoint, string? detail)
    {
        Console.WriteLine(
            $"START endpoint={endpoint} detail={detail ?? "-"} tid={Thread.CurrentThread.ManagedThreadId} " +
            $"pool={Thread.CurrentThread.IsThreadPoolThread} at={DateTime.UtcNow:O}");
    }

    private static void LogEnd(string endpoint, long elapsedMs, string? detail)
    {
        Console.WriteLine(
            $"END endpoint={endpoint} detail={detail ?? "-"} elapsedMs={elapsedMs} tid={Thread.CurrentThread.ManagedThreadId} " +
            $"pool={Thread.CurrentThread.IsThreadPoolThread} at={DateTime.UtcNow:O}");
    }
}
