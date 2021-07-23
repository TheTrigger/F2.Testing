using Microsoft.AspNetCore.Mvc;
using Oibi.Demo;
using Oibi.Demo.Controllers;
using Oibi.TestHelper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static Oibi.Demo.Controllers.WeatherForecastController;

namespace Oibi.Tests
{
	/// <summary>
	/// Basic configuration
	/// </summary>
	public class ClientTests : IClassFixture<ServerFixture<Startup>>
	{
		private readonly ServerFixture<Startup> _testFixure;
		private readonly WeatherForecastController _controller;

		public ClientTests(ServerFixture<Startup> testFixture)
		{
			_testFixure = testFixture;
			_controller = _testFixure.GetService<WeatherForecastController>();
		}

		[Fact]
		public async Task IsGetWorking()
		{
			var results = await _testFixure.GetAsync<IEnumerable<WeatherForecast>>("WeatherForecast");

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

			var result = await _testFixure.PostAsync<DataExample>("WeatherForecast", dataExample);

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

			var result = await _testFixure.PutAsync<DataExample>("WeatherForecast", dataExample);

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
}