using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FewBox.Core.Utility.Net
{
    public static class HttpClientExtension
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent httpContent)
        {
            return await VerbAsync(client, requestUri, "PATCH", httpContent);
        }

        public static async Task<HttpResponseMessage> HeadAsync(this HttpClient client, string requestUri)
        {
            return await VerbAsync(client, requestUri, "HEAD");
        }

        public static async Task<HttpResponseMessage> ConnectAsync(this HttpClient client, string requestUri)
        {
            return await VerbAsync(client, requestUri, "CONNECT");
        }

        public static async Task<HttpResponseMessage> OptionsAsync(this HttpClient client, string requestUri)
        {
            return await VerbAsync(client, requestUri, "OPTIONS");
        }

        public static async Task<HttpResponseMessage> TraceAsync(this HttpClient client, string requestUri)
        {
            return await VerbAsync(client, requestUri, "TRACE");
        }

        private static async Task<HttpResponseMessage> VerbAsync(HttpClient client, string requestUri, string verb, HttpContent httpContent)
        {
            var method = new HttpMethod(verb);
            var request = new HttpRequestMessage(method, requestUri);
            if(httpContent != null)
            {
                request.Content = httpContent;
            }

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await client.SendAsync(request);
            }
            catch (TaskCanceledException taskCanceledException)
            {
                Debug.WriteLine("ERROR: " + taskCanceledException.ToString());
            }

            return response;
        }

        private static async Task<HttpResponseMessage> VerbAsync(HttpClient client, string requestUri, string verb)
        {
            return await VerbAsync(client, requestUri, verb, null);
        }
    }
}