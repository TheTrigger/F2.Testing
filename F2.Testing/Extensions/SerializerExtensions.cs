using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace F2.Testing
{
    public static class SerializerExtensions
    {
        public static JsonSerializerOptions JsonOptions { get; set; } = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        /// <summary>
        /// Serialize an <see cref="object"/> to <see cref="StringContent"/> : <see cref="HttpContent"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static StringContent ToStringContent(this object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.Default, System.Net.Mime.MediaTypeNames.Application.Json);
        }

        /// <summary>
        /// <inheritdoc cref="JsonSerializer.DeserializeAsync{TValue}(System.IO.Stream, JsonSerializerOptions?, System.Threading.CancellationToken)"/>
        /// </summary>
        public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// <inheritdoc cref="JsonSerializer.DeserializeAsync{TValue}(System.IO.Stream, JsonSerializerOptions?, System.Threading.CancellationToken)"/>
        /// </summary>
        public static async Task<T> DeserializeAsync<T>(this Task<HttpResponseMessage> task, CancellationToken cancellationToken = default)
        {
            var response = await task.ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}