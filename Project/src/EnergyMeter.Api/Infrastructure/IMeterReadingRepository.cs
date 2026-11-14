using EnergyMeter.Api.Domain;

namespace EnergyMeter.Api.Infrastructure;

/// <summary>Storage abstraction so a persistent database can replace the demo store.</summary>
public interface IMeterReadingRepository
{
    bool TryAdd(MeterReading reading);
    IReadOnlyList<MeterReading> FindByMeter(string meterId);
}
