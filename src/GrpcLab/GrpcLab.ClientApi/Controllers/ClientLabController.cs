using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using GrpcLab.Protos;
using Microsoft.AspNetCore.Mvc;

namespace GrpcLab.ClientApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientLabController : ControllerBase
{
    private readonly Grpc.Net.ClientFactory.GrpcClientFactory _clientFactory;
    private readonly ILogger<ClientLabController> _logger;

    // Direct channel for manual manipulation
    private readonly string _serverAddress = "http://grpc-server:8080";

    public ClientLabController(Grpc.Net.ClientFactory.GrpcClientFactory clientFactory, ILogger<ClientLabController> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    [HttpPost("stream-backpressure")]
    public async Task<IActionResult> StreamBackpressure([FromQuery] int readDelayMs = 0)
    {
        // Manual channel creation for control
        using var channel = GrpcChannel.ForAddress(_serverAddress);
        var client = new ExperimentsService.ExperimentsServiceClient(channel);

        using var call = client.StreamData(new StreamRequest { MsgSize = 10000, ItemCount = 10000 });
        
        int count = 0;
        var start = DateTime.UtcNow;

        try 
        {
            await foreach (var msg in call.ResponseStream.ReadAllAsync())
            {
                count++;
                if (count % 100 == 0) Console.WriteLine($"[Client] Received {count}");
                
                // SLOW READER: Causes Backpressure
                if (readDelayMs > 0)
                {
                    await Task.Delay(readDelayMs);
                }
            }
        }
        catch(RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.Cancelled)
        {
             _logger.LogWarning("Stream cancelled");
        }

        return Ok(new { Received = count, Duration = (DateTime.UtcNow - start).TotalSeconds });
    }

    [HttpPost("connection-churn")]
    public async Task<IActionResult> ConnectionChurn([FromQuery] int iterations = 100)
    {
        int success = 0;
        var start = DateTime.UtcNow;

        for (int i = 0; i < iterations; i++)
        {
            // BAD PRACTICE: Creating channel per request!
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new ExperimentsService.ExperimentsServiceClient(channel);
            
            await client.UnaryWorkAsync(new WorkRequest { LoadIntensity = 1, Info = "Churn" });
            success++;
            // Channel Disposed immediately
        }

        return Ok(new { Iterations = success, Duration = (DateTime.UtcNow - start).TotalSeconds });
    }

    [HttpPost("retry-storm")]
    public async Task<IActionResult> RetryStorm()
    {
        // Use the Factory client which should have Retry Policy configured in Program.cs
        // But for this lab, let's manually configure a channel with aggressive retry
        
        var methodConfig = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 50, // AGGRESSIVE!
                InitialBackoff = TimeSpan.FromMilliseconds(10), // NO BACKOFF!
                MaxBackoff = TimeSpan.FromMilliseconds(100),
                BackoffMultiplier = 1,
                RetryableStatusCodes = { Grpc.Core.StatusCode.Unavailable }
            }
        };

        using var channel = GrpcChannel.ForAddress(_serverAddress, new GrpcChannelOptions
        {
            ServiceConfig = new ServiceConfig { MethodConfigs = { methodConfig } }
        });
        
        var client = new ExperimentsService.ExperimentsServiceClient(channel);

        try 
        {
            await client.FlakyUnaryAsync(new WorkRequest { LoadIntensity = 10 });
            return Ok("Success after retries");
        }
        catch (RpcException ex)
        {
            return BadRequest($"Failed after retries: {ex.Status}");
        }
    }
}
