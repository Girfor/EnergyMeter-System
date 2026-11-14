using EnergyMeter.Api.Infrastructure;

namespace EnergyMeter.Api.Domain;

/// <summary>Coordinates input validation and atomic registration of a meter reading.</summary>
public sealed class MeterReadingService(
    IMeterReadingRepository repository, TimeProvider timeProvider)
{
    public CreateReadingResult Create(CreateReadingRequest request)
    {
        var error = MeterReadingValidator.Validate(request, timeProvider.GetUtcNow());
        if (error is not null)
            return new(CreateStatus.Invalid, Error: error);

        var reading = new MeterReading(Guid.NewGuid(), request!.MeterId.Trim(),
            request.ValueKwh, request.ObservedAtUtc.ToUniversalTime());
        return repository.TryAdd(reading)
            ? new(CreateStatus.Created, reading)
            : new(CreateStatus.Duplicate, Error: "Reading already exists.");
    }

    public IReadOnlyList<MeterReading> GetByMeter(string meterId) =>
        repository.FindByMeter(meterId);
}
