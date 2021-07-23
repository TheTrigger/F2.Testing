using Microsoft.Extensions.Logging;
using Oibi.Demo;
using Oibi.Demo.Services;
using Oibi.TestHelper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Oibi.Tests
{
	/// <summary>
	/// Basic configuration
	/// </summary>
	public class DemoTests : IClassFixture<ServerFixture<Startup>>
	{
		private readonly ServerFixture<Startup> _testFixure;
		private readonly RouteAnalyzer _routeAnalyzer;
		private readonly RandomService _randomService;

		public DemoTests(ServerFixture<Startup> testFixture)
		{
			_testFixure = testFixture;
			_routeAnalyzer = _testFixure.GetService<RouteAnalyzer>();
			_randomService = _testFixure.GetService<RandomService>();
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
			Assert.NotNull(_testFixure.GetService<RouteAnalyzer>());
			Assert.NotNull(_testFixure.GetService<ILogger<DemoTests>>());

			var rdns = _testFixure.GetService<RandomService>();
			Assert.NotNull(rdns);

			var number = rdns.GetRandomNumber();
			Assert.True(number >= 0);
		}
	}
}