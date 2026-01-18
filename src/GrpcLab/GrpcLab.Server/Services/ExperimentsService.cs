using Grpc.Core;
using GrpcLab.Protos;

namespace GrpcLab.Server.Services;

public class ExperimentsService : GrpcLab.Protos.ExperimentsService.ExperimentsServiceBase
{
    private readonly ILogger<ExperimentsService> _logger;
    private static int _flakyCounter = 0;

    public ExperimentsService(ILogger<ExperimentsService> logger)
    {
        _logger = logger;
    }

    public override async Task<WorkResponse> UnaryWork(WorkRequest request, ServerCallContext context)
    {
        // Scenario 1: Unary Saturation
        // Simulate CPU/IO load based on intensity
        var delay = request.LoadIntensity > 0 ? request.LoadIntensity : 10;
        
        await Task.Delay(delay);

        return new WorkResponse
        {
            Success = true,
            Message = $"Processed work with intensity {delay}ms. Info: {request.Info}"
        };
    }

    public override async Task StreamData(StreamRequest request, IServerStreamWriter<DataMessage> responseStream, ServerCallContext context)
    {
        // Scenario 2: Streaming Backpressure
        // We will try to push as fast as possible.
        // If client is slow, HTTP/2 flow control kicks in.
        // The write to responseStream will eventually block or buffer depending on implementation.
        
        int seq = 0;
        var payload = Google.Protobuf.ByteString.CopyFrom(new byte[request.MsgSize > 0 ? request.MsgSize : 1024]);
        
        // Endless stream or count based
        while (!context.CancellationToken.IsCancellationRequested)
        {
            if (request.ItemCount > 0 && seq >= request.ItemCount) break;

            await responseStream.WriteAsync(new DataMessage
            {
                SeqId = ++seq,
                Payload = payload,
                Timestamp = DateTime.UtcNow.ToString("O")
            });

            // No artificial delay here! We want to swamp the connection.
        }
    }

    public override async Task<WorkResponse> FlakyUnary(WorkRequest request, ServerCallContext context)
    {
        // Scenario 3: Retry Storm
        // Fail 50% of the time to trigger retries
        var count = Interlocked.Increment(ref _flakyCounter);
        
        if (count % 2 != 0)
        {
            throw new RpcException(new Status(StatusCode.Unavailable, "Transient failure - Try again!"));
        }

        return await UnaryWork(request, context);
    }
}
