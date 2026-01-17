
namespace StorageLab.OrderApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using StorageLab.OrderApi.Services;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("experiments/storage/deadlock")]
public class DeadlockController : ControllerBase
{
    private readonly OrderContext _context;
    
    public DeadlockController(OrderContext context)
    {
        _context = context;
    }

    [HttpGet("bad")]
    public async Task<IActionResult> BadScenario()
    {
        // SCENARIO 1: BAD (Deadlock Prone)
        // Transaction A locks Resource 1 then tries for 2
        // Transaction B locks Resource 2 then tries for 1
        // We simulate this by forcing an order that conflicts with another concurrent request if timed right.
        
        var sw = Stopwatch.StartNew();
        
        try 
        {
            // Simulate Transaction A logic (Lock Id 1 then 2)
            await using var transaction = await _context.Database.BeginTransactionAsync();
            
            var p1 = await _context.Products.FromSqlRaw("SELECT * FROM \"Products\" WHERE \"Id\" = 1 FOR UPDATE").FirstOrDefaultAsync();
            await Task.Delay(1000); // Artificial delay to increase deadlock chance
            var p2 = await _context.Products.FromSqlRaw("SELECT * FROM \"Products\" WHERE \"Id\" = 2 FOR UPDATE").FirstOrDefaultAsync();
            
            p1.Stock--;
            p2.Stock--;
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            sw.Stop();
            await ResultLogger.LogResultAsync("Deadlock_Bad", new { Status = "Success", Latency = sw.ElapsedMilliseconds }, "Transaction completed without deadlock (Lucky!)");
            return Ok("Transaction A success");
        }
        catch (Exception ex)
        {
            sw.Stop();
            await ResultLogger.LogResultAsync("Deadlock_Bad", new { Status = "Failed", Error = ex.Message, Latency = sw.ElapsedMilliseconds }, "Deadlock occurred! (Expected behavior)");
            return Problem(detail: ex.Message, title: "Deadlock Detected");
        }
    }

    [HttpGet("good")]
    public async Task<IActionResult> GoodScenario()
    {
        // SCENARIO 2: GOOD (Deadlock Free)
        // Always lock resources in the SAME ORDER (e.g. by ID)
        
        var sw = Stopwatch.StartNew();
        try 
        {
             await using var transaction = await _context.Database.BeginTransactionAsync();
             
             // Always lock smallest ID first
             var p1 = await _context.Products.FromSqlRaw("SELECT * FROM \"Products\" WHERE \"Id\" = 1 FOR UPDATE").FirstOrDefaultAsync();
             var p2 = await _context.Products.FromSqlRaw("SELECT * FROM \"Products\" WHERE \"Id\" = 2 FOR UPDATE").FirstOrDefaultAsync();

             p1.Stock--;
             p2.Stock--;

             await _context.SaveChangesAsync();
             await transaction.CommitAsync();

             sw.Stop();
             await ResultLogger.LogResultAsync("Deadlock_Good", new { Status = "Success", Latency = sw.ElapsedMilliseconds }, "Consistent locking order prevented deadlock.");
             return Ok("Transaction B success");
        }
        catch (Exception ex)
        {
             sw.Stop();
             await ResultLogger.LogResultAsync("Deadlock_Good", new { Status = "Failed", Error = ex.Message }, "Unexpected failure.");
             return Problem(ex.Message);
        }
    }
}

// Helper classes moved to bottom
public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
}
