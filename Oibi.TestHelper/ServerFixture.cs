using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Oibi.TestHelper
{
	public class ServerFixture<TStartup> : /*IAsyncLifetime,*/ IDisposable where TStartup : class
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
		/// Instantiate a test server with asppsettings and environment valiables
		/// </summary>
		public ServerFixture()
		{
			var builder = new WebHostBuilder() //.UseEnvironment("Development")
					.ConfigureAppConfiguration((_, config) =>
					{
						config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
						config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
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

		/// <summary>
		/// <inheritdoc cref="HttpClient.GetAsync"/>
		/// </summary>
		public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
		{
			var request = await Client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
			return await request.DeserializeBodyAsync<T>().ConfigureAwait(false);
		}

		/// <summary>
		/// <inheritdoc cref="HttpClient.PostAsync"/>
		/// </summary>
		public async Task<T> PostAsync<T>(string requestUri, object data, CancellationToken cancellationToken = default)
		{
			var request = await Client.PostAsync(requestUri, data.ToStringContent(), cancellationToken).ConfigureAwait(false);
			return await request.DeserializeBodyAsync<T>().ConfigureAwait(false);
		}

		public async Task<T> PutAsync<T>(string requestUri, object data)
		{
			var request = await Client.PutAsync(requestUri, data.ToStringContent()).ConfigureAwait(false);
			return await request.DeserializeBodyAsync<T>().ConfigureAwait(false);
		}

		/// <summary>
		/// <inheritdoc cref="HttpClient.DeleteAsync(string?, CancellationToken)"/>
		/// </summary>
		/// <typeparam name="T">Expected return type</typeparam>
		/// <param name="requestUri"><inheritdoc cref="HttpClient.DeleteAsync(string?, CancellationToken)"/></param>
		/// <returns><inheritdoc cref="HttpClient.DeleteAsync(string?, CancellationToken)"/></returns>
		public async Task<T> DeleteAsync<T>(string requestUri, CancellationToken cancellationToken = default)
		{
			var request = await Client.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
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

		//public virtual Task InitializeAsync()
		//{
		//	return Task.CompletedTask;
		//	//throw new NotImplementedException();
		//}

		//public virtual Task DisposeAsync()
		//{
		//	return Task.CompletedTask;
		//	//throw new NotImplementedException();
		//}
	}
}