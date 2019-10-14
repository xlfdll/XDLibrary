using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xlfdll.Collections;

namespace Xlfdll.Network
{
    public static class WebOperations
    {
        public static String GetContentAsString(String url)
        {
            return WebOperations.GetContentAsString(url, Encoding.UTF8, null);
        }

        public static String GetContentAsString(String url, Encoding encoding)
        {
            return WebOperations.GetContentAsString(url, encoding, null);
        }

        public static String GetContentAsString(String url, Encoding encoding, IEnumerable<KeyValuePair<String, String>> query)
        {
            Uri uri = WebOperations.GenerateUri(url, query);

            String result = String.Empty;

            using (WebClient client = new WebClient() { Encoding = encoding })
            {
                result = client.DownloadString(uri);
            }

            return WebUtility.HtmlDecode(result);
        }

        public static async Task<String> GetContentAsStringAsync(String url)
        {
            return await WebOperations.GetContentAsStringAsync(url, Encoding.UTF8, null);
        }

        public static async Task<String> GetContentAsStringAsync(String url, Encoding encoding)
        {
            return await WebOperations.GetContentAsStringAsync(url, encoding, null);
        }

        public static async Task<String> GetContentAsStringAsync(String url, Encoding encoding, IEnumerable<KeyValuePair<String, String>> query)
        {
            Uri uri = WebOperations.GenerateUri(url, query);

            String result = String.Empty;

            using (WebClient client = new WebClient() { Encoding = encoding })
            {
                result = await client.DownloadStringTaskAsync(uri);
            }

            return WebUtility.HtmlDecode(result);
        }

        private static Uri GenerateUri(String url, IEnumerable<KeyValuePair<String, String>> query)
        {
            Uri uri = null;

            if (query != null)
            {
                UriBuilder ub = new UriBuilder(url);
                StringBuilder sb = new StringBuilder();

                query.ForEach
                (
                    (pair) =>
                    {
                        sb.AppendFormat("{0}={1}&", pair.Key, pair.Value);
                    }
                );

                sb.Remove(sb.Length - 1, 1);

                ub.Query = Uri.EscapeUriString(sb.ToString());

                uri = ub.Uri;
            }
            else
            {
                uri = new Uri(url);
            }

            return uri;
        }
    }
}