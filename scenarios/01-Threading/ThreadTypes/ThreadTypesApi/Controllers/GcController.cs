using Microsoft.AspNetCore.Mvc;
using ThreadTypesApi.Services;
using ThreadTypesApi.Services.Gc;

namespace ThreadTypesApi.Controllers;

[ApiController]
[Route("gc")]
public class GcController : ControllerBase
{
    private readonly GcLab _gcLab;

    public GcController(GcLab gcLab)
    {
        _gcLab = gcLab;
    }

    [HttpPost("standard")]
    public IActionResult RunStandard()
    {
        var report = _gcLab.RunStandardScenario();
        return Ok(report);
    }

    [HttpPost("finalizer")]
    public IActionResult RunFinalizer()
    {
        var report = _gcLab.RunFinalizerScenario();
        return Ok(report);
    }
}
