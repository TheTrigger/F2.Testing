using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Net.Http;

namespace Oibi.TestHelper
{
    public class ServerFixture<TStartup> : IDisposable where TStartup : class
    {
        /// <summary>
        /// Current <see cref="TStartup"/> server
        /// </summary>
        public TestServer Server { get; private set; }

        /// <summary>
        /// Your <see cref="HttpClient"/>
        /// </summary>
        public HttpClient Client { get; private set; }

        /// <summary>
        /// Get service by type
        /// </summary>
        public TService GetService<TService>() => (TService)Server.Host.Services.GetService(typeof(TService));

        public ICollection GetRoutesOfController() => throw new NotImplementedException();
        public ICollection GetRoutesOfControllerMethod() => throw new NotImplementedException();

        /// <summary>
        /// Instantiate a test server with asppsettings or environment valiables
        /// </summary>
        public ServerFixture()
        {
            IWebHostBuilder builder = new WebHostBuilder().UseEnvironment("Development")
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                        config.AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true);
                        config.AddEnvironmentVariables();
                    })
                    .UseStartup<TStartup>();

            // only 1 build allowed WebHost = builder.Build();
            Server = new TestServer(builder);
            Client = Server.CreateClient();
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
