using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace F2.Testing.Extensions
{
    /// <summary>
    /// <inheritdoc cref="HttpResponseMessage"/>
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        public static Cookie? GetCookie(this HttpResponseMessage httpResponseMessage, string name)
        {
            ArgumentNullException.ThrowIfNull(httpResponseMessage);
            ArgumentNullException.ThrowIfNull(name);

            if (!httpResponseMessage.Headers.TryGetValues("Set-Cookie", out var setCookieHeaders)) return null;

            foreach (var setCookieString in setCookieHeaders)
            {
                var cookie = new Cookie();

                var pairs = setCookieString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var raw in pairs)
                {
                    var values = raw.Split('=', StringSplitOptions.TrimEntries);
                    if (values.Length == 0) throw new ArgumentException("Unable to parse cookie");
                    var keyValue = KeyValuePair.Create(values[0], (values.Length >= 2) ? values[1] : null);


                    if (keyValue.Key.Equals(nameof(Cookie.HttpOnly), StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.HttpOnly = true;
                    }

                    else if (keyValue.Key.Equals(nameof(Cookie.Secure), StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.Secure = true;
                    }

                    else if (keyValue.Key.Equals(nameof(Cookie.Domain), StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.Domain = keyValue.Value;
                    }

                    else if (keyValue.Key.Equals(nameof(Cookie.Path), StringComparison.InvariantCultureIgnoreCase))
                    {
                        cookie.Path = keyValue.Value;
                    }

                    else if (keyValue.Key.Equals(nameof(Cookie.Expires), StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (DateTime.TryParse(keyValue.Value, out var result))
                        {
                            cookie.Expires = result;
                        }
                    }

                    else if (keyValue.Key.Equals("samesite", StringComparison.InvariantCultureIgnoreCase))
                    {
                    }

                    else if (string.IsNullOrWhiteSpace(cookie.Name) && string.IsNullOrWhiteSpace(cookie.Value))
                    {
                        cookie.Name = keyValue.Key;
                        cookie.Value = keyValue.Value;
                    }

                    else
                    {
                        // unrecognized
                    }
                }

                if (cookie.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return cookie;
            }

            return null;
        }
    }
}