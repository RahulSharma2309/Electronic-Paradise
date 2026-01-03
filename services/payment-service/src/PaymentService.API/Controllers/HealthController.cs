using Microsoft.AspNetCore.Mvc;

namespace PaymentService.API.Controllers;

/// <summary>
/// Provides health check endpoints for the Payment service.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Checks the health of the Payment service.
    /// </summary>
    /// <returns>An OK response with service status.</returns>
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Healthy", service = "payment-service" });
}
