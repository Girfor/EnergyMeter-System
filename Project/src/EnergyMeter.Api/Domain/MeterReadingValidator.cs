using System.Text.RegularExpressions;

namespace EnergyMeter.Api.Domain;

/// <summary>Small, independently testable checks for incoming readings.</summary>
public static class MeterReadingValidator
{
    private static readonly Regex MeterIdPattern = new(
        "^[A-Z0-9-]{3,24}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static string? Validate(CreateReadingRequest? request, DateTimeOffset now)
    {
        if (request is null) return "Request is required.";
        return ValidateId(request.MeterId)
            ?? ValidateValue(request.ValueKwh)
            ?? ValidateTimestamp(request.ObservedAtUtc, now);
    }

    private static string? ValidateId(string? meterId)
    {
        if (string.IsNullOrWhiteSpace(meterId)) return "MeterId is required.";
        return MeterIdPattern.IsMatch(meterId.Trim()) ? null : "MeterId format is invalid.";
    }

    private static string? ValidateValue(decimal value) =>
        value is >= 0m and <= 1_000_000m ? null : "ValueKwh is out of range.";

    private static string? ValidateTimestamp(DateTimeOffset observed, DateTimeOffset now) =>
        observed > now.AddMinutes(5) || observed < now.AddYears(-5)
            ? "ObservedAtUtc is out of range." : null;
}
