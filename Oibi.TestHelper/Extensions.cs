using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Oibi.TestHelper
{
    internal static class Extensions
    {
        private const string mediaType = "application/json";

        public static JsonSerializerOptions JsonOptions { get; set; } = new JsonSerializerOptions
        {
            IgnoreNullValues = false,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        /// <summary>
        /// Serialize an <see cref="object"/> to <see cref="StringContent"/> : <see cref="HttpContent"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static StringContent ToStringContent(this object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.Default, mediaType);
        }

        /// <summary>
        /// Deserialize the response to <see cref="T"/> <see cref="System.Type"/>
        /// </summary>
        internal static async Task<T> DeserializeBodyAsync<T>(this HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions).ConfigureAwait(false);
        }
    }
}