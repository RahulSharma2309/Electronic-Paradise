// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HealthController.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers;

/// <summary>
/// Provides health check endpoints for the user service.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Gets the health status of the user service.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the health status.</returns>
    /// <response code="200">Returns the health status indicating the service is healthy.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok(new { status = "Healthy", service = "user-service" });
}
