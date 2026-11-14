using System.Collections.Concurrent;
using EnergyMeter.Api.Domain;

namespace EnergyMeter.Api.Infrastructure;

/// <summary>Thread-safe educational storage; data are lost on application restart.</summary>
public sealed class InMemoryMeterReadingRepository : IMeterReadingRepository
{
    private readonly ConcurrentDictionary<(string, DateTimeOffset), MeterReading> _store = new();

    public bool TryAdd(MeterReading reading) =>
        _store.TryAdd((reading.MeterId, reading.ObservedAtUtc), reading);

    public IReadOnlyList<MeterReading> FindByMeter(string meterId) =>
        _store.Values.Where(r => r.MeterId == meterId)
            .OrderBy(r => r.ObservedAtUtc).ToList();
}
