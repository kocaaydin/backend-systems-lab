using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Serilog;

namespace BackendLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ObservabilityController : ControllerBase
{
    // Metrics
    private static readonly Meter MyMeter = new("BackendLab.Observability", "1.0.0");
    private static readonly Counter<long> RequestCounter = MyMeter.CreateCounter<long>("obs_request_count");
    private static readonly Histogram<double> LatencyHistogram = MyMeter.CreateHistogram<double>("obs_latency_ms");

    [HttpGet("trace")]
    public async Task<IActionResult> GenerateTrace()
    {
        // Start a new activity (trace span)
        using var activity = new ActivitySource("BackendLab.Observability").StartActivity("ManualTraceGen");
        
        activity?.SetTag("custom.tag", "trace-demo");
        activity?.AddEvent(new ActivityEvent("Processing started"));

        await Task.Delay(150); // Simulate work

        activity?.AddEvent(new ActivityEvent("Processing finished"));
        
        return Ok(new { TraceId = activity?.TraceId.ToString(), Message = "Trace generated and sent to Jaeger." });
    }

    [HttpGet("metric")]
    public IActionResult GenerateMetric()
    {
        // Increment counter
        RequestCounter.Add(1, new KeyValuePair<string, object?>("endpoint", "metric"));
        
        // Record histogram
        var simulatedLatency = Random.Shared.Next(50, 500);
        LatencyHistogram.Record(simulatedLatency);

        return Ok(new { Message = "Metric incremented.", Value = simulatedLatency });
    }

    [HttpGet("log")]
    public IActionResult GenerateLog()
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? "no-trace";
        
        // Log Structured Data
        Log.Information("Manual log generation requested. TraceId: {TraceId}. User: {User}", traceId, "DemoUser");
        Log.Warning("This is a sample warning log for demonstration.");
        
        return Ok(new { Message = "Logs written to Loki.", TraceId = traceId });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GenerateAll()
    {
        using var activity = new ActivitySource("BackendLab.Observability").StartActivity("GenerateAllSignals");
        
        // 1. Trace
        activity?.SetTag("demo.signal", "all");
        await Task.Delay(50);
        
        // 2. Metric
        RequestCounter.Add(1, new KeyValuePair<string, object?>("endpoint", "all"));
        LatencyHistogram.Record(100);

        // 3. Log
        Log.Information("Generating ALL signals. TraceId: {TraceId}", activity?.TraceId);

        return Ok(new { 
            Message = "All signals generated!",
            TraceId = activity?.TraceId.ToString()
        });
    }
}
