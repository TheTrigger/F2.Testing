using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Oibi.TestHelper
{
    public static class Extensions
    {
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
        public static StringContent ToStringContent(this object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.Default, System.Net.Mime.MediaTypeNames.Application.Json);
        }

        /// <summary>
        /// Deserialize the response to <see cref="T"/> <see cref="System.Type"/>
        /// </summary>
        public static async Task<T> DeserializeBodyAsync<T>(this HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions).ConfigureAwait(false);
        }
    }
}