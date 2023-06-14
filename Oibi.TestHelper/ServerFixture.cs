using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Oibi.TestHelper;

public class ServerFixture<TStartup> : WebApplicationFactory<TStartup> /*, IHostedService,*/ where TStartup : class
{
    /// <summary>
    /// Your <see cref="HttpClient"/>
    /// </summary>
    public HttpClient Client { get; private set; }
    private bool _clientConfigured = false;

    //public ICollection GetRoutesOfController() => throw new NotImplementedException();
    //public ICollection GetRoutesOfControllerMethod() => throw new NotImplementedException();

    private IServiceScope _scope;

    /// <summary>
    /// Get generic service
    /// </summary>
    public TService GetService<TService>()
    {
        _scope ??= Server.Services.CreateScope();
        return _scope.ServiceProvider.GetService<TService>();
    }

    /// <summary>
    /// Get generic services
    /// </summary>
    public IEnumerable<TService> GetServices<TService>()
    {
        _scope ??= Server.Services.CreateScope();
        return _scope.ServiceProvider.GetServices<TService>();
    }


    /// <summary>
    /// Instantiate a test server with asppsettings and environment valiables
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true);
        })
        .ConfigureLogging((_, logging) =>
        {
            logging.AddDebug();
        })
        .ConfigureServices(services =>
        {
            //services.AddHostedService<ServerFixture<TStartup>>();
            //services.AddSingleton<RouteAnalyzer>();
        })
        .UseStartup<TStartup>()
        ;

        if (_clientConfigured == false)
        {
            _clientConfigured = true;
            Client = CreateClient();
        }
    }

    /// <summary>
    /// <inheritdoc cref="HttpClient.GetAsync"/>
    /// <inheritdoc cref="SerializerExtensions.DeserializeAsync{T}(HttpResponseMessage, CancellationToken)"/>
    /// </summary>
    public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        var request = await Client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        return await request.DeserializeAsync<T>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// <inheritdoc cref="HttpClient.PostAsync"/>
    /// <inheritdoc cref="SerializerExtensions.DeserializeAsync{T}(HttpResponseMessage, CancellationToken)"/>
    /// </summary>
    public async Task<T> PostAsync<T>(string requestUri, object data, CancellationToken cancellationToken = default)
    {
        var request = await Client.PostAsync(requestUri, data.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await request.DeserializeAsync<T>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// <inheritdoc cref="HttpClient.PutAsync(string?, HttpContent)"/>
    /// <inheritdoc cref="SerializerExtensions.DeserializeAsync{T}(HttpResponseMessage, CancellationToken)"/>
    /// </summary>
    public async Task<T> PutAsync<T>(string requestUri, object data, CancellationToken cancellationToken = default)
    {
        var request = await Client.PutAsync(requestUri, data.ToStringContent(), cancellationToken).ConfigureAwait(false);
        return await request.DeserializeAsync<T>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// <inheritdoc cref="HttpClient.DeleteAsync(string?, CancellationToken)"/>
    /// <inheritdoc cref="SerializerExtensions.DeserializeAsync{T}(HttpResponseMessage, CancellationToken)"/>
    /// </summary>
    public async Task<T> DeleteAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        var request = await Client.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
        return await request.DeserializeAsync<T>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// <inheritdoc cref="HttpClient.GetAsync"/>
    /// </summary>
    public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default)
    {
        return Client.GetAsync(requestUri, cancellationToken);
    }

    /// <summary>
    /// <inheritdoc cref="HttpClient.PostAsync"/>
    /// </summary>
    public Task<HttpResponseMessage> PostAsync(string requestUri, object data, CancellationToken cancellationToken = default)
    {
        return Client.PostAsync(requestUri, data.ToStringContent(), cancellationToken);
    }

    /// <summary>
    /// <inheritdoc cref="HttpClient.PutAsync"/>
    /// </summary>
    public Task<HttpResponseMessage> PutAsync(string requestUri, object data, CancellationToken cancellationToken = default)
    {
        return Client.PutAsync(requestUri, data.ToStringContent(), cancellationToken);
    }

    /// <summary>
    /// <inheritdoc cref="HttpClient.DeleteAsync(string?, CancellationToken)"/>
    /// </summary>
    public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken = default)
    {
        return Client.DeleteAsync(requestUri, cancellationToken);
    }
}

