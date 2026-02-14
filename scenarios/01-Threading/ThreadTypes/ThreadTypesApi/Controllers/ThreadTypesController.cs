using System.Diagnostics;
using System.Threading.Channels;
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
        var sw = Stopwatch.StartNew();
        LogStart("io-async", $"delayMs={delayMs}");

        await Task.Delay(delayMs, cancellationToken);

        sw.Stop();
        LogEnd("io-async", sw.ElapsedMilliseconds, $"delayMs={delayMs}");
        return Ok(new
        {
            endpoint = "io-async",
            delayMs,
            elapsedMs = sw.ElapsedMilliseconds,
            managedThreadId = Thread.CurrentThread.ManagedThreadId,
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread
        });
    }

    [HttpGet("cpu-heavy-threadpool")]
    public async Task<IActionResult> CpuHeavyThreadPool([FromServices] CpuCalculator calculator, [FromQuery] int n = 20000)
    {
        var sw = Stopwatch.StartNew();
        LogStart("cpu-heavy-threadpool", $"n={n}");

        var result = await Task.Run(() => calculator.CountPrimes(n));

        sw.Stop();
        LogEnd("cpu-heavy-threadpool", sw.ElapsedMilliseconds, $"n={n}");
        return Ok(new
        {
            endpoint = "cpu-heavy-threadpool",
            n,
            primeCount = result,
            elapsedMs = sw.ElapsedMilliseconds,
            managedThreadId = Thread.CurrentThread.ManagedThreadId,
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread
        });
    }

    [HttpGet("cpu-heavy-dedicated")]
    public async Task<IActionResult> CpuHeavyDedicated([FromServices] CpuCalculator calculator, [FromQuery] int n = 20000)
    {
        var sw = Stopwatch.StartNew();
        LogStart("cpu-heavy-dedicated", $"n={n}");

        var tcs = new TaskCompletionSource<(int PrimeCount, int ThreadId, bool IsPool)>(TaskCreationOptions.RunContinuationsAsynchronously);
        var thread = new Thread(() =>
        {
            try
            {
                var count = calculator.CountPrimes(n);
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
        LogEnd("cpu-heavy-dedicated", sw.ElapsedMilliseconds, $"n={n}");
        return Ok(new
        {
            endpoint = "cpu-heavy-dedicated",
            n,
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
        var sw = Stopwatch.StartNew();
        LogStart("starvation/blocking", $"blockMs={blockMs}");

        Thread.Sleep(blockMs);

        sw.Stop();
        LogEnd("starvation/blocking", sw.ElapsedMilliseconds, $"blockMs={blockMs}");
        return Ok(new
        {
            endpoint = "starvation/blocking",
            blockMs,
            elapsedMs = sw.ElapsedMilliseconds,
            managedThreadId = Thread.CurrentThread.ManagedThreadId,
            isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread
        });
    }

    [HttpGet("starvation/blocking-dedicated")]
    public async Task<IActionResult> StarvationBlockingDedicated([FromQuery] int blockMs = 3000)
    {
        var sw = Stopwatch.StartNew();
        LogStart("starvation/blocking-dedicated", $"blockMs={blockMs}");

        var tcs = new TaskCompletionSource<(int ThreadId, bool IsPool)>(TaskCreationOptions.RunContinuationsAsynchronously);
        var thread = new Thread(() =>
        {
            Thread.Sleep(blockMs);
            tcs.SetResult((Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread));
        })
        {
            IsBackground = true,
            Name = "blocking-dedicated-thread"
        };
        
        thread.Start();
        var result = await tcs.Task;

        sw.Stop();
        LogEnd("starvation/blocking-dedicated", sw.ElapsedMilliseconds, $"blockMs={blockMs}");
        return Ok(new
        {
            endpoint = "starvation/blocking-dedicated",
            blockMs,
            elapsedMs = sw.ElapsedMilliseconds,
            requestThreadId = Thread.CurrentThread.ManagedThreadId,
            requestThreadIsPool = Thread.CurrentThread.IsThreadPoolThread,
            workerThreadId = result.ThreadId,
            workerThreadIsPool = result.IsPool
        });
    }

    [HttpGet("cpu-cancellable")]
    public IActionResult CpuCancellable([FromServices] CpuCalculator calculator, [FromQuery] int n = 250000, [FromQuery] int checkEvery = 200, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        LogStart("cpu-cancellable", $"n={n},checkEvery={checkEvery}");

        try
        {
            var count = calculator.CountPrimesCancellable(n, cancellationToken, checkEvery);
            sw.Stop();
            LogEnd("cpu-cancellable", sw.ElapsedMilliseconds, $"n={n},completed=true");
            return Ok(new
            {
                endpoint = "cpu-cancellable",
                n,
                checkEvery,
                primeCount = count,
                elapsedMs = sw.ElapsedMilliseconds,
                cancelled = false
            });
        }
        catch (OperationCanceledException)
        {
            sw.Stop();
            LogEnd("cpu-cancellable", sw.ElapsedMilliseconds, $"n={n},completed=false");
            return StatusCode(499, new
            {
                endpoint = "cpu-cancellable",
                n,
                elapsedMs = sw.ElapsedMilliseconds,
                cancelled = true
            });
        }
    }

    [HttpPost("queue/enqueue")]
    public async Task<IActionResult> Enqueue([FromQuery] int items = 20, [FromQuery] int capacity = 5, [FromQuery] int workMs = 300, CancellationToken cancellationToken = default)
    {
        LogStart("queue/enqueue", $"items={items},capacity={capacity},workMs={workMs}");

        if (items <= 0 || capacity <= 0 || workMs <= 0)
        {
            return BadRequest(new { error = "items, capacity ve workMs pozitif olmalı." });
        }

        var channel = Channel.CreateBounded<int>(new BoundedChannelOptions(capacity)
        {
            SingleReader = true,
            SingleWriter = true,
            FullMode = BoundedChannelFullMode.Wait
        });

        var processed = 0;
        var producerWaitTotalMs = 0L;
        var producerWaitMaxMs = 0L;

        var consumer = Task.Run(async () =>
        {
            await foreach (var _ in channel.Reader.ReadAllAsync(cancellationToken))
            {
                await Task.Delay(workMs, cancellationToken);
                processed++;
            }
        }, cancellationToken);

        for (var i = 0; i < items; i++)
        {
            var waitSw = Stopwatch.StartNew();
            await channel.Writer.WriteAsync(i, cancellationToken);
            waitSw.Stop();
            producerWaitTotalMs += waitSw.ElapsedMilliseconds;
            producerWaitMaxMs = Math.Max(producerWaitMaxMs, waitSw.ElapsedMilliseconds);
        }

        channel.Writer.Complete();
        await consumer;

        LogEnd("queue/enqueue", 0, $"items={items},processed={processed},producerWaitMaxMs={producerWaitMaxMs}");
        return Ok(new
        {
            endpoint = "queue/enqueue",
            items,
            capacity,
            workMs,
            backpressureDetected = producerWaitMaxMs > 0,
            producerWaitMs = producerWaitMaxMs,
            note = "producerWaitMs > 0 ise producer kuyruk doldugunda beklemistir."
        });
    }

    [HttpPost("finalizer/create")]
    public IActionResult CreateFinalizerSamples([FromQuery] int count = 10000)
    {
        LogStart("finalizer/create", $"count={count}");

        for (var i = 0; i < count; i++)
        {
            _ = new FinalizerProbe();
        }

        var snapshot = FinalizerProbe.Snapshot();
        LogEnd("finalizer/create", 0, $"created={snapshot.Created},finalized={snapshot.Finalized}");
        return Ok(new
        {
            endpoint = "finalizer/create",
            count,
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
