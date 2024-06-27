# F2.Testing

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/e6e207b4981a49a0b624a882f78954c0)](https://www.codacy.com/gh/TheTrigger/Oibi.TestHelper/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=TheTrigger/Oibi.TestHelper&amp;utm_campaign=Badge_Grade)

```CSharp
using F2.Testing;
using Xunit;

namespace F2.Testing.Tests
{
    /// <summary>
    /// Basic configuration
    /// </summary>
    public class DemoTests : IClassFixture<ServerFixture<Startup>>
    {
        private readonly ServerFixture<Startup> _testFixure;
        private readonly WeatherForecastController _controller;

        public DemoTests(ServerFixture<Startup> testFixture)
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
    }
}

```

### Refs

- <https://docs.microsoft.com/it-it/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1?view=aspnetcore-5.0>
- <https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2>

