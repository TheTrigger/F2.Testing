using F2.Testing;
using F2.Testing.Demo;
using F2.Testing.Demo.Controllers;
using F2.Testing.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace F2.Testing.Tests;

/// <summary>
/// Basic configuration
/// </summary>
public class ClientTests : IClassFixture<ServerFixture<Startup>>
{
    private readonly ServerFixture<Startup> _testFixture;

    public ClientTests(ServerFixture<Startup> testFixture)
    {
        _testFixture = testFixture;
    }

    [Fact]
    public async Task IsGetWorking()
    {
        var results = await _testFixture.Client.GetAsync("WeatherForecast").DeserializeAsync<IEnumerable<WeatherForecast>>();
        Assert.NotNull(results);
        Assert.NotEmpty(results);
    }

    [Fact]
    public async Task IsPostWorking()
    {
        var dataExample = new DataExample
        {
            Age = 12,
            BirthDay = System.DateTime.Now,
            Name = "Fabio"
        };

        var result = await _testFixture.Client.PostAsync("WeatherForecast", dataExample.ToStringContent())
                .DeserializeAsync<DataExample>();

        Assert.NotNull(result);
        Assert.IsType<DataExample>(result);
        Assert.Equal(dataExample.Age, result.Age);
        Assert.Equal(dataExample.BirthDay, result.BirthDay);
        Assert.Equal(dataExample.Name, result.Name);
    }

    [Fact]
    public async Task IsPutWorking()
    {
        var dataExample = new DataExample
        {
            Age = 12,
            BirthDay = System.DateTime.Now,
            Name = "Fabio"
        };

        var result = await _testFixture.PutAsync<DataExample>("WeatherForecast", dataExample);

        Assert.NotNull(result);
        Assert.IsType<DataExample>(result);
        Assert.Equal(dataExample.Age, result.Age);
        Assert.Equal(dataExample.BirthDay, result.BirthDay);
        Assert.Equal(dataExample.Name, result.Name);
    }


    [Fact]
    public async Task IsDeleteWorking()
    {
        var results = await _testFixture.DeleteAsync<dynamic>("WeatherForecast");

        Assert.NotNull(results);
    }
}