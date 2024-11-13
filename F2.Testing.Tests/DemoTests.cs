using Microsoft.Extensions.Logging;
using F2.Demo;
using F2.Demo.Services;
using F2.Testing;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace F2.Tests;

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
        Assert.NotNull(_testFixure.GetService<ILogger<DemoTests>>());

        var rdns = _testFixure.GetService<RandomService>();
        Assert.NotNull(rdns);

        var number = rdns.GetRandomNumber();
        Assert.True(number >= 0);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}