using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Oibi.TestHelper
{
    public class ServerFixture<TStartup> : IDisposable where TStartup : class
    {
        /// <summary>
        /// Current <see cref="TStartup"/> server
        /// </summary>
        public TestServer Server { get; }

        /// <summary>
        /// Your <see cref="HttpClient"/>
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Get service by type
        /// </summary>
        public TService GetService<TService>() => (TService)Server.Host.Services.GetService(typeof(TService));

        //public ICollection GetRoutesOfController() => throw new NotImplementedException();
        //public ICollection GetRoutesOfControllerMethod() => throw new NotImplementedException();

        /// <summary>
        /// Instantiate a test server with asppsettings or environment valiables
        /// </summary>
        public ServerFixture()
        {
            IWebHostBuilder builder = new WebHostBuilder() //.UseEnvironment("Development")
                    .ConfigureAppConfiguration((_, config) =>
                    {
                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                        config.AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true);
                        config.AddEnvironmentVariables();
                    })
                    .ConfigureLogging((_, logging) =>
                    {
                        logging.AddDebug();
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton<RouteAnalyzer>();
                    })
                    .UseStartup<TStartup>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            var request = await Client.GetAsync(requestUri).ConfigureAwait(false);
            return await request.DeserializeBodyAsync<T>().ConfigureAwait(false);
        }

        public async Task<T> PostAsync<T>(string requestUri, T data)
        {
            var request = await Client.PostAsync(requestUri, data.ToStringContent()).ConfigureAwait(false);
            return await request.DeserializeBodyAsync<T>().ConfigureAwait(false);
        }

        public async Task<T> PutAsync<T>(string requestUri, T data)
        {
            var request = await Client.PutAsync(requestUri, data.ToStringContent()).ConfigureAwait(false);
            return await request.DeserializeBodyAsync<T>().ConfigureAwait(false);
        }

        public async Task<T> DeleteAsync<T>(string requestUri)
        {
            var request = await Client.DeleteAsync(requestUri).ConfigureAwait(false);
            return await request.DeserializeBodyAsync<T>().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client?.Dispose();
                Server?.Dispose();
            }
        }
    }
}