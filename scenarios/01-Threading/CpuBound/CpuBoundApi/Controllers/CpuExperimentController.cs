using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("experiments")]
public sealed class CpuExperimentController : ControllerBase
{
    [HttpGet("cpu")]
    public IActionResult Run([FromServices] CpuCalculator calculator, [FromQuery] int n = 20000)
    {
        var safe = Math.Clamp(n, 1000, 100000);
        var primeCount = calculator.CountPrimes(safe);

        return Ok(new
        {
            input = safe,
            primeCount,
            note = "CPU-bound brute-force prime counting"
        });
    }
}
