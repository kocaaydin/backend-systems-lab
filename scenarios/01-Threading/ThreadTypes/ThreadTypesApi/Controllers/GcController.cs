using Microsoft.AspNetCore.Mvc;
using ThreadTypesApi.Services.Gc;

namespace ThreadTypesApi.Controllers;

[ApiController]
[Route("gc")]
public class GcController(GcLab gcLab) : ControllerBase
{
    [HttpPost("standard")]
    public IActionResult RunStandard() => Ok(gcLab.RunStandardScenario());

    [HttpPost("finalizer")]
    public IActionResult RunFinalizer() => Ok(gcLab.RunFinalizerScenario());

    [HttpPost("generations")]
    public IActionResult RunGenerations() => Ok(gcLab.RunGenerationsScenario());

    [HttpPost("freeze/small")]
    public IActionResult RunSmallFreeze() => Ok(gcLab.RunSmallObjectFreeze());

    [HttpPost("freeze/large")]
    public IActionResult RunLargeFreeze() => Ok(gcLab.RunLargeObjectFreeze());
}
