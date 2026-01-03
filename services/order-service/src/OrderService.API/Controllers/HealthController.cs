using Microsoft.AspNetCore.Mvc;

namespace OrderService.API.Controllers;

/// <summary>
/// Provides health check endpoints for the Order service.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Checks the health of the Order service.
    /// </summary>
    /// <returns>An OK response with service status.</returns>
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Healthy", service = "order-service" });
}
