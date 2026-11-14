using EnergyMeter.Api.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyMeter.Api.Controllers;

/// <summary>Cookie-authenticated dashboard endpoint, protected from CSRF.</summary>
[ApiController]
[Route("api/dashboard/readings")]
[Authorize]
[AutoValidateAntiforgeryToken]
public sealed class DashboardController(MeterReadingService service) : ControllerBase
{
    // The host identity provider/login UI is intentionally out of scope.
    // Once cookie auth is connected, browser clients must submit a valid token.
    [HttpPost]
    public IActionResult CreateManual([FromBody] CreateReadingRequest request)
    {
        var result = service.Create(request);
        return result.Status switch
        {
            CreateStatus.Created => Ok(result.Reading),
            CreateStatus.Duplicate => Conflict(new { error = result.Error }),
            _ => BadRequest(new { error = result.Error })
        };
    }
}
