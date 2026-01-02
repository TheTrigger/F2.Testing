using F2.Testing.Demo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace F2.Testing.Tests;

/// <summary>
/// Basic configuration
/// </summary>
public class SettingsTests : IClassFixture<ServerFixture<Startup>>
{
    private readonly ServerFixture<Startup> _testFixture;
    private readonly IConfiguration _configuration;
    private readonly ITestOutputHelper _output;

    public SettingsTests(ServerFixture<Startup> testFixture, ITestOutputHelper output)
    {
        _testFixture = testFixture;
        _configuration = testFixture.Services.GetRequiredService<IConfiguration>();
        _output = output;
    }

    [Fact]
    public void VerifyCurrentSettingsTest()
    {
        Assert.NotNull(_configuration);
        var currentSettings = _configuration["CurrentSettings"];

        var prodLoaded = _configuration["ProdLoaded"];
        var devLoaded = _configuration["DevLoaded"];
        var testLoaded = _configuration["TestLoaded"];

        // Output per debug - mostra quale valore è stato effettivamente caricato
        _output.WriteLine($"===========================================");
        _output.WriteLine($"CurrentSettings letto: '{currentSettings}'");
        _output.WriteLine($"===========================================");
        _output.WriteLine($"  PROD = appsettings.json (base)");
        _output.WriteLine($"  DEV  = appsettings.Development.json (sovrascrive base)");
        _output.WriteLine($"  TEST = appsettings.test.json (sovrascrive tutto)");
        _output.WriteLine($"===========================================");

        Assert.NotNull(currentSettings);
        Assert.Equal("TEST", currentSettings); // deve sovrascrivere DEV e PROD
        Assert.Equal("True", prodLoaded);
        Assert.Equal("True", devLoaded);
        Assert.Equal("True", testLoaded);
    }
}