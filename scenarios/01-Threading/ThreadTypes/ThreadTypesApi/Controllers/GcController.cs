using Microsoft.AspNetCore.Mvc;
using ThreadTypesApi.Services;
using ThreadTypesApi.Services.Gc;

namespace ThreadTypesApi.Controllers;

[ApiController]
[Route("gc")]
public class GcController(GcLab gcLab) : ControllerBase
{
    private readonly GcLab _gcLab = gcLab;

    [HttpPost("standard")]
    public IActionResult RunStandard() => Ok(_gcLab.RunStandardScenario());

    [HttpPost("finalizer")]
    public IActionResult RunFinalizer() => Ok(_gcLab.RunFinalizerScenario());

    [HttpPost("generations")]
    public IActionResult RunGenerations() => Ok(_gcLab.RunGenerationsScenario());

    [HttpPost("freeze/small")]
    public IActionResult RunSmallFreeze() => Ok(_gcLab.RunSmallObjectFreeze());

    [HttpPost("freeze/large")]
    public IActionResult RunLargeFreeze() => Ok(_gcLab.RunLargeObjectFreeze());
}
