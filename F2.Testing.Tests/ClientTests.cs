using F2.Demo;
using F2.Demo.Controllers;
using F2.Testing;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static F2.Demo.Controllers.WeatherForecastController;

namespace F2.Tests;

/// <summary>
/// Basic configuration
/// </summary>
public class ClientTests : IClassFixture<ServerFixture<Startup>>
{
	private readonly ServerFixture<Startup> _testFixure;
	private readonly WeatherForecastController _controller;
	private readonly HttpClient _client;

	public ClientTests(ServerFixture<Startup> testFixture)
	{
		_testFixure = testFixture;
		_controller = _testFixure.GetService<WeatherForecastController>();
		_client = _testFixure.CreateClient(); // CLASSIC METHOD
	}

	[Fact]
	public async Task IsGetWorking()
	{
            var results = await _client.GetAsync("WeatherForecast") 
			.DeserializeAsync<IEnumerable<WeatherForecast>>();
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

		var result = await _client.PostAsync("WeatherForecast", dataExample.ToStringContent())
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

		var result = await _testFixure.PutAsync<DataExample>("WeatherForecast", dataExample); // ALTERNATIVE METHOD

            Assert.NotNull(result);
		Assert.IsType<DataExample>(result);
		Assert.Equal(dataExample.Age, result.Age);
		Assert.Equal(dataExample.BirthDay, result.BirthDay);
		Assert.Equal(dataExample.Name, result.Name);
	}


	[Fact]
	public async Task IsDeleteWorking()
	{
		var results = await _testFixure.DeleteAsync<dynamic>("WeatherForecast");

		Assert.NotNull(results);
	}
}