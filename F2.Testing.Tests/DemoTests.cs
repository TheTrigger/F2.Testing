using F2.Testing.Demo;
using F2.Testing.Demo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace F2.Testing.Tests;

/// <summary>
/// Basic configuration
/// </summary>
public class DemoTests : IClassFixture<ServerFixture<Startup>>, IAsyncLifetime
{
    private readonly ServerFixture<Startup> _testFixure;
    private bool _isInitialized = false;
    //private readonly RouteAnalyzer _routeAnalyzer;
    //private readonly RandomService _randomService;

    public DemoTests(ServerFixture<Startup> testFixture)
    {
        _testFixure = testFixture;
        //_routeAnalyzer = _testFixure.GetService<RouteAnalyzer>();
        //_randomService = _testFixure.GetService<RandomService>();
    }

    public Task InitializeAsync()
    {
        _isInitialized = true;
        return Task.CompletedTask;
    }

    [Fact]
    public Task IsInitialized()
    {
        Assert.True(_isInitialized);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetWeatherForecast()
    {
        var results = await _testFixure.GetAsync<IEnumerable<WeatherForecast>>("WeatherForecast");

        Assert.NotEmpty(results);
    }

    [Fact]
    public void GetServices()
    {
        // analyzer is autoinjected while testing
        //Assert.NotNull(_testFixure.GetService<RouteAnalyzer>());
        Assert.NotNull(_testFixure.Services.GetRequiredService<ILogger<DemoTests>>());

        var rdns = _testFixure.Services.GetRequiredService<RandomService>();
        Assert.NotNull(rdns);

        var number = rdns.GetRandomNumber();
        Assert.True(number >= 0);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}