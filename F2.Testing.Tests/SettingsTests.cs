using F2.Demo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace F2.Testing.Tests;

/// <summary>
/// Basic configuration
/// </summary>
public class SettingsTests : IClassFixture<ServerFixture<Startup>>
{
    private readonly ServerFixture<Startup> _testFixture;
    private readonly IConfiguration _configuration;

    public SettingsTests(ServerFixture<Startup> testFixture)
    {
        _testFixture = testFixture;
        _configuration = testFixture.Services.GetRequiredService<IConfiguration>();
    }

    [Fact]
    public async Task VerifyCurrentSettingsTest()
    {
        Assert.NotNull(_configuration);
        var currentSettings = _configuration["CurrentSettings"];

        Assert.Equal("TEST", currentSettings);
        await Task.CompletedTask;
    }
}