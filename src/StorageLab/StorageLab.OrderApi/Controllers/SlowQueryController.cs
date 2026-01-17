using Microsoft.AspNetCore.Mvc;
using StorageLab.OrderApi.Services;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace StorageLab.OrderApi.Controllers;

[ApiController]
[Route("experiments/storage/slow-query")]
public class SlowQueryController : ControllerBase
{
    private readonly OrderContext _context;

    public SlowQueryController(OrderContext context)
    {
        _context = context;
    }

    [HttpGet("bad")]
    public async Task<IActionResult> BadScenario([FromQuery] int delayMs = 2000)
    {
        // SCENARIO 1: BAD (N+1 Problem & Missing Index Simulation)
        var sw = Stopwatch.StartNew();
        
        // Simulating a slow query by fetching all and filtering in memory (BAD)
        // or just forcing a delay inside a transaction to hold locks
        
        try 
        {
             // For simulation, we just Wait. In real EF, this would be a bad LINQ query.
             await Task.Delay(delayMs); 
             
             // Also simulate fetching WAY too much data
             var heavyData = Enumerable.Range(1, 10000).Select(x => new Product { Id = x, Name = $"Product {x}", Stock = 100 }).ToList();

             sw.Stop();
             await ResultLogger.LogResultAsync("SlowQuery_Bad", new { Status = "Success", Latency = sw.ElapsedMilliseconds, Rows = heavyData.Count }, "Query took too long and fetched excessive data.");
             return Ok($"Fetched {heavyData.Count} rows in {sw.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("good")]
    public async Task<IActionResult> GoodScenario([FromQuery] int delayMs = 2000)
    {
        // SCENARIO 2: GOOD (Pagination & Indexed Query)
        var sw = Stopwatch.StartNew();

        try 
        {
             // Efficient query
             await Task.Delay(50); // Fast DB response
             
             // Fetch only what is needed (Pagination)
             var lightData = Enumerable.Range(1, 20).Select(x => new Product { Id = x, Name = $"Product {x}", Stock = 100 }).ToList();

             sw.Stop();
             await ResultLogger.LogResultAsync("SlowQuery_Good", new { Status = "Success", Latency = sw.ElapsedMilliseconds, Rows = lightData.Count }, "Efficient query with pagination.");
             return Ok($"Fetched {lightData.Count} rows in {sw.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
