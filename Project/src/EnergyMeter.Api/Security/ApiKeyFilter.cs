using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EnergyMeter.Api.Security;

/// <summary>Rejects device requests without an explicitly supplied API key.</summary>
public sealed class ApiKeyFilter(IConfiguration configuration) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var configured = configuration["EnergyMeter:ApiKey"];
        if (string.IsNullOrWhiteSpace(configured))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
            return;
        }

        var provided = context.HttpContext.Request.Headers["X-API-Key"].ToString();
        var expectedBytes = Encoding.UTF8.GetBytes(configured);
        var providedBytes = Encoding.UTF8.GetBytes(provided);
        if (expectedBytes.Length != providedBytes.Length ||
            !CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes))
        {
            context.Result = new UnauthorizedObjectResult(new { error = "Unauthorized" });
            return;
        }

        await next();
    }
}
