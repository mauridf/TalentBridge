using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/Health")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult HealthCheck()
    {
        _logger.LogInformation("GET /api/Health");
        return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
    }
}
