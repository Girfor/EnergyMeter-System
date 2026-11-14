namespace EnergyMeter.Api.Domain;

/// <summary>A cumulative energy-meter reading recorded at a UTC instant.</summary>
public sealed record MeterReading(
    Guid Id, string MeterId, decimal ValueKwh, DateTimeOffset ObservedAtUtc);

/// <summary>Input sent by an energy-meter device or an authorized operator.</summary>
public sealed record CreateReadingRequest(
    string MeterId, decimal ValueKwh, DateTimeOffset ObservedAtUtc);

public enum CreateStatus { Created, Invalid, Duplicate }

public sealed record CreateReadingResult(
    CreateStatus Status, MeterReading? Reading = null,
    string? Error = null);
