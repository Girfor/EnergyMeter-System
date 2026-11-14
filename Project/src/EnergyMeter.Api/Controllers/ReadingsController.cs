using EnergyMeter.Api.Domain;
using EnergyMeter.Api.Security;
using Microsoft.AspNetCore.Mvc;

namespace EnergyMeter.Api.Controllers;

/// <summary>API for ingesting and retrieving energy-meter readings.</summary>
[ApiController]
[Route("api/readings")]
[ServiceFilter(typeof(ApiKeyFilter))]
public sealed class ReadingsController(MeterReadingService service) : ControllerBase
{
    /// <summary>Registers a new reading; duplicate meter/timestamp pairs are rejected.</summary>
    [HttpPost]
    public IActionResult Create([FromBody] CreateReadingRequest request)
    {
        var result = service.Create(request);
        return result.Status switch
        {
            CreateStatus.Created => CreatedAtAction(nameof(GetByMeter),
                new { meterId = result.Reading!.MeterId }, result.Reading),
            CreateStatus.Duplicate => Conflict(new { error = result.Error }),
            _ => BadRequest(new { error = result.Error })
        };
    }

    /// <summary>Returns readings belonging to a specified energy meter.</summary>
    [HttpGet("{meterId}")]
    public IActionResult GetByMeter(string meterId) =>
        Ok(service.GetByMeter(meterId));
}
