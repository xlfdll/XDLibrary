using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xlfdll.Collections;

namespace Xlfdll.Network
{
    public static class WebOperations
    {
        private static HttpClient HttpClient { get; } = new HttpClient();

        public static String GetContentAsString(String url)
        {
            String content = WebOperations.HttpClient.GetStringAsync(url).Result;

            return WebUtility.HtmlDecode(content);
        }

        public static String GetContentAsString(String url, Encoding encoding)
        {
            String content = encoding.GetString(WebOperations.HttpClient.GetByteArrayAsync(url).Result);

            return WebUtility.HtmlDecode(content);
        }

        public static String GetContentAsString(String url, IEnumerable<KeyValuePair<String, String>> queries)
        {
            Uri uri = WebOperations.GenerateUri(url, queries);
            String content = WebOperations.HttpClient.GetStringAsync(uri).Result;

            return WebUtility.HtmlDecode(content);
        }

        public static String GetContentAsString(String url, IEnumerable<KeyValuePair<String, String>> queries, Encoding encoding)
        {
            Uri uri = WebOperations.GenerateUri(url, queries);
            String content = encoding.GetString(WebOperations.HttpClient.GetByteArrayAsync(uri).Result);

            return WebUtility.HtmlDecode(content);
        }

        public static async Task<String> GetContentAsStringAsync(String url)
        {
            String content = await WebOperations.HttpClient.GetStringAsync(url);

            return WebUtility.HtmlDecode(content);
        }

        public static async Task<String> GetContentAsStringAsync(String url, Encoding encoding)
        {
            String content = encoding.GetString(await WebOperations.HttpClient.GetByteArrayAsync(url));

            return WebUtility.HtmlDecode(content);
        }

        public static async Task<String> GetContentAsStringAsync(String url, IEnumerable<KeyValuePair<String, String>> queries)
        {
            Uri uri = WebOperations.GenerateUri(url, queries);
            String content = await WebOperations.HttpClient.GetStringAsync(uri);

            return WebUtility.HtmlDecode(content);
        }

        public static async Task<String> GetContentAsStringAsync(String url, IEnumerable<KeyValuePair<String, String>> queries, Encoding encoding)
        {
            Uri uri = WebOperations.GenerateUri(url, queries);
            String content = encoding.GetString(await WebOperations.HttpClient.GetByteArrayAsync(uri));

            return WebUtility.HtmlDecode(content);
        }

        public static Uri GenerateUri(String url, IEnumerable<KeyValuePair<String, String>> queries)
        {
            Uri uri = null;

            if (queries != null)
            {
                UriBuilder ub = new UriBuilder(url);
                StringBuilder sb = new StringBuilder();

                queries.ForEach
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