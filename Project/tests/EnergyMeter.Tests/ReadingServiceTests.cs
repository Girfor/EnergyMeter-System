using EnergyMeter.Api.Domain;
using EnergyMeter.Api.Infrastructure;
using FluentAssertions;
using Xunit;

namespace EnergyMeter.Tests;

public sealed class ReadingServiceTests
{
    private static MeterReadingService NewService() =>
        new(new InMemoryMeterReadingRepository(), TimeProvider.System);

    [Fact]
    public void ValidReading_ShouldBeCreated()
    {
        var result = NewService().Create(new("METER-001", 120.5m, DateTimeOffset.UtcNow));
        result.Status.Should().Be(CreateStatus.Created);
        result.Reading!.ValueKwh.Should().Be(120.5m);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1000000.01)]
    public void OutOfRangeReading_ShouldBeRejected(double value)
    {
        var result = NewService().Create(new("METER-001", (decimal)value, DateTimeOffset.UtcNow));
        result.Status.Should().Be(CreateStatus.Invalid);
    }

    [Fact]
    public void DuplicateTimestamp_ShouldReturnConflict()
    {
        var service = NewService();
        var request = new CreateReadingRequest("METER-001", 100m, DateTimeOffset.UtcNow);
        service.Create(request).Status.Should().Be(CreateStatus.Created);
        service.Create(request).Status.Should().Be(CreateStatus.Duplicate);
    }

    [Fact]
    public void InvalidMeterId_ShouldBeRejected()
    {
        NewService().Create(new("bad meter", 100m, DateTimeOffset.UtcNow))
            .Status.Should().Be(CreateStatus.Invalid);
    }

    [Fact]
    public void FutureReading_ShouldBeRejected()
    {
        NewService().Create(new("METER-001", 100m, DateTimeOffset.UtcNow.AddDays(1)))
            .Status.Should().Be(CreateStatus.Invalid);
    }
}
