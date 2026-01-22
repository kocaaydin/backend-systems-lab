
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using BackendLab.Api.Handlers;
using BackendLab.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. LOGGING (Serilog -> OTLP Collector)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = "http://otel-collector:4317";
        options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
        options.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = builder.Configuration["OTEL_SERVICE_NAME"] ?? "backend-lab-api"
        };
    })
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "backend-lab-api-{0:yyyy.MM.dd}",
        NumberOfShards = 2,
        NumberOfReplicas = 1
    })
    .CreateLogger();

builder.Host.UseSerilog();

// 2. OPEN TELEMETRY (Tracing + Metrics)
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(
        serviceName: builder.Configuration["OTEL_SERVICE_NAME"] ?? "backend-lab-api",
        serviceVersion: "1.0.0"))
    .WithTracing(tracing => tracing
        .AddSource("BackendLab.Observability") // Source for manual traces
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter()) // Jaeger uses OTLP
    .WithMetrics(metrics => metrics
        .AddMeter("BackendLab.Observability") // Meter for manual metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter()
        .AddPrometheusExporter());

// 3. MVC / CONTROLLERS
builder.Services.AddControllers();

// REGISTER SERVICES
builder.Services.AddHostedService<ThreadStarvationBackgroundService>();
builder.Services.AddTransient<ClientRateLimitingHandler>(sp => new ClientRateLimitingHandler(100)); // Limit 100 RPS
builder.Services.AddHttpClient("RateLimitedClient").AddHttpMessageHandler<ClientRateLimitingHandler>();

var app = builder.Build();

app.UseRouting();


// Middleware to capture Scenario Header as Trace Tag
app.Use(async (context, next) =>
{
    var activity = System.Diagnostics.Activity.Current;
    if (activity != null && context.Request.Headers.TryGetValue("X-Test-Scenario", out var scenario))
    {
        activity.SetTag("test.scenario", scenario.ToString());
        System.Diagnostics.Activity.Current?.AddBaggage("test.scenario", scenario.ToString());
    }
    await next();
});

// Prometheus metrics middleware
app.UseOpenTelemetryPrometheusScrapingEndpoint();

// Map Controllers
app.MapControllers();



app.Run();

// Helper Handler (kept for compatibility)

