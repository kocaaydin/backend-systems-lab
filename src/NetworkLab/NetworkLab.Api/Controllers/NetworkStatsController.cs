using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace NetworkLab.Api.Controllers;

[ApiController]
[Route("api/network")]
public class NetworkStatsController : ControllerBase
{
    private readonly ILogger<NetworkStatsController> _logger;

    public NetworkStatsController(ILogger<NetworkStatsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("tcp-stats")]
    public IActionResult GetTcpStats()
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var tcpStats = ipGlobalProperties.GetTcpIPv4Statistics();
        
        var stats = new
        {
            ActiveConnections = ipGlobalProperties.GetActiveTcpConnections().Length,
            TcpStats = new
            {
                CurrentConnections = tcpStats.CurrentConnections,
                CumulativeConnections = tcpStats.CumulativeConnections,
                ConnectionsInitiated = tcpStats.ConnectionsInitiated,
                ConnectionsAccepted = tcpStats.ConnectionsAccepted,
                FailedConnectionAttempts = tcpStats.FailedConnectionAttempts,
                ResetConnections = tcpStats.ResetConnections,
                ErrorsReceived = tcpStats.ErrorsReceived,
                SegmentsSent = tcpStats.SegmentsSent,
                SegmentsReceived = tcpStats.SegmentsReceived,
                SegmentsResent = tcpStats.SegmentsResent
            },
            Timestamp = DateTime.UtcNow
        };

        return Ok(stats);
    }

    [HttpGet("active-connections")]
    public IActionResult GetActiveConnections()
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var connections = ipGlobalProperties.GetActiveTcpConnections();

        var groupedConnections = connections
            .GroupBy(c => c.State)
            .Select(g => new
            {
                State = g.Key.ToString(),
                Count = g.Count(),
                Connections = g.Take(10).Select(c => new
                {
                    LocalEndpoint = c.LocalEndPoint.ToString(),
                    RemoteEndpoint = c.RemoteEndPoint.ToString(),
                    State = c.State.ToString()
                }).ToList()
            })
            .ToList();

        return Ok(new
        {
            TotalConnections = connections.Length,
            ByState = groupedConnections,
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("port-usage")]
    public IActionResult GetPortUsage()
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var tcpListeners = ipGlobalProperties.GetActiveTcpListeners();
        var udpListeners = ipGlobalProperties.GetActiveUdpListeners();

        return Ok(new
        {
            TcpListeners = tcpListeners.Select(l => new
            {
                Port = l.Port,
                Address = l.Address.ToString()
            }).OrderBy(l => l.Port).ToList(),
            UdpListeners = udpListeners.Select(l => new
            {
                Port = l.Port,
                Address = l.Address.ToString()
            }).OrderBy(l => l.Port).ToList(),
            TotalTcpPorts = tcpListeners.Length,
            TotalUdpPorts = udpListeners.Length,
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("ephemeral-ports")]
    public IActionResult GetEphemeralPortInfo()
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var connections = ipGlobalProperties.GetActiveTcpConnections();
        
        // Ephemeral port range (typically 49152-65535)
        var ephemeralConnections = connections
            .Where(c => c.LocalEndPoint.Port >= 49152)
            .ToList();

        var timeWaitConnections = connections
            .Where(c => c.State == System.Net.NetworkInformation.TcpState.TimeWait)
            .ToList();

        return Ok(new
        {
            EphemeralPortsInUse = ephemeralConnections.Count,
            TimeWaitConnections = timeWaitConnections.Count,
            TotalAvailableEphemeralPorts = 65535 - 49152 + 1, // 16384 ports
            PortExhaustionRisk = ephemeralConnections.Count > 10000 ? "HIGH" : 
                                 ephemeralConnections.Count > 5000 ? "MEDIUM" : "LOW",
            Details = new
            {
                EphemeralRange = "49152-65535",
                CurrentUsage = ephemeralConnections.Count,
                TimeWaitCount = timeWaitConnections.Count,
                AvailablePorts = (65535 - 49152 + 1) - ephemeralConnections.Count
            },
            Timestamp = DateTime.UtcNow
        });
    }
}
