using Oibi.Demo;
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

        public DemoTests(ServerFixture<Startup> testFixture)
        {
            _testFixure = testFixture;
        }

        [Fact]
        public async Task Test1()
        {
            var e = await _testFixure.GetAsync<IEnumerable<WeatherForecast>>("WeatherForecast");

            Assert.NotEmpty(e);
        }
    }
}