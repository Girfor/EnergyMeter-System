using System.Net;
using System.Net.Http.Json;
using EnergyMeter.Api.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EnergyMeter.Tests;

public sealed class ReadingsApiTests
{
    private sealed class TestApp : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Production");
            builder.ConfigureAppConfiguration((_, config) =>
                config.AddInMemoryCollection(new Dictionary<string, string?>
                { ["EnergyMeter:ApiKey"] = "test-api-key" }));
        }
    }

    [Fact]
    public async Task MissingApiKey_ShouldReturnUnauthorized()
    {
        using var factory = new TestApp();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        { BaseAddress = new Uri("https://localhost") });
        var response = await client.GetAsync("/api/readings/METER-001");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ValidReading_ShouldReturnCreated()
    {
        using var factory = new TestApp();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        { BaseAddress = new Uri("https://localhost") });
        client.DefaultRequestHeaders.Add("X-API-Key", "test-api-key");
        var request = new CreateReadingRequest("METER-002", 87.1m, DateTimeOffset.UtcNow);
        var response = await client.PostAsJsonAsync("/api/readings", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ProductionSwagger_ShouldNotBeAvailable()
    {
        using var factory = new TestApp();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        { BaseAddress = new Uri("https://localhost"), AllowAutoRedirect = false });
        var response = await client.GetAsync("/swagger/v1/swagger.json");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
