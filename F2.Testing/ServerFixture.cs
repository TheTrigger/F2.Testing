using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace F2.Testing;

public class ServerFixture<TStartup> : WebApplicationFactory<TStartup>, IAsyncLifetime where TStartup : class
{
    /// <summary>
    /// Your <see cref="HttpClient"/>
    /// </summary>
    public HttpClient Client { get; internal set; }
    public CookieContainer CookieContainer { get; private set; } = new CookieContainer();

    /// <summary>
    /// Provides access to services within the created scope. 
    /// Use this to resolve scoped services
    /// </summary>
    public IServiceProvider ScopedServices => _scope.ServiceProvider;

    private IServiceScope _scope;
    private HttpClientHandler _handler;

    public ServerFixture()
    {
        //_handler = new HttpClientHandler
        //{
        //    CookieContainer = CookieContainer,
        //    UseCookies = true,
        //};

        //var delegateHandler = new DelegatingHandler { InnerHandler = _handler };

        //Client = this.CreateClient(new WebApplicationFactoryClientOptions { Handler = delegatingHandler });
        //Client = CreateDefaultClient(_handler);
    }

    [Obsolete("Use .Services or .ScopedServices")]
    /// <inheritdoc cref="ServiceProviderServiceExtensions.GetService{T}(IServiceProvider)"/>
    public TService GetService<TService>()
    {
        return Services.GetService<TService>();
    }

    [Obsolete("Use .Services or .ScopedServices")]
    /// <inheritdoc cref="ServiceProviderServiceExtensions.GetRequiredService{T}(IServiceProvider)"/>
    public TService GetRequiredService<TService>()
    {
        return Services.GetRequiredService<TService>();
    }

    [Obsolete("Use .Services or .ScopedServices")]
    /// <inheritdoc cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/>
    public IEnumerable<TService> GetServices<TService>()
    {
        return Services.GetServices<TService>();
    }

    /// <summary>
    /// Instantiate a test server with asppsettings and environment valiables
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder //.UseStartup<TStartup>() already called by WebApplicationFactory
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile(Path.Join(Directory.GetCurrentDirectory(), "appsettings.test.json"), optional: true, reloadOnChange: true);
            })
            .ConfigureLogging((_, logging) =>
            {
                logging.AddDebug();
            })
            .ConfigureServices(services =>
            {
            })
        ;
    }

    #region HTTP HELPERS

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

    #endregion

    public virtual Task InitializeAsync()
    {
        Client = CreateClient();
        _scope = Services.CreateScope();
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return base.DisposeAsync().AsTask();
    }
}

