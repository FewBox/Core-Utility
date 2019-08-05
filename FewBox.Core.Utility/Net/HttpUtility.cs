using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FewBox.Core.Utility.Net
{
    public static class HttpUtility
    {
        public static bool IsCertificateNeedValidate { private get; set; }
        public static bool IsLogging { private get; set; }
        public static bool IsEnsureSuccessStatusCode { private get; set; }
        public static TimeSpan Timeout { private get; set; }

        static HttpUtility()
        {
            IsLogging = false;
            IsCertificateNeedValidate = true;
            IsEnsureSuccessStatusCode = true;
            Timeout = TimeSpan.FromMinutes(1);
        }

        #region Normal


        public static string Get(string url, IList<Header> headers)
        {
            return WapperHttpClient((httpClient) =>
            {
                return httpClient.GetAsync(url);
            }, headers);
        }

        public static string Get(string url, string token, IList<Header> headers)
        {
            return WapperHttpClientWithToken((httpClient) =>
            {
                return httpClient.GetAsync(url);
            }, token, headers);
        }

        # endregion

        private static void InitHeadersObjectToHttpRequestHeaders(HttpClient httpClient, IList<Header> headers)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            httpClient.DefaultRequestHeaders.UserAgent.Clear();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("FewBox/0.1 (Linux x86_64)");
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        private static string WapperHttpClient(Func<HttpClient, Task<HttpResponseMessage>> action, IList<Header> headers)
        {
            return WapperHttpClientWithToken(action, String.Empty, headers);
        }

        private static string WapperHttpClientWithToken(Func<HttpClient, Task<HttpResponseMessage>> action, string token, IList<Header> headers)
        {
            using (var handler = new HttpClientHandler())
            {
                if (!IsCertificateNeedValidate)
                {
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                }
                using (HttpClient httpClient = new HttpClient(handler))
                {
                    httpClient.Timeout = Timeout;
                    if (!String.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    InitHeadersObjectToHttpRequestHeaders(httpClient, headers);
                    return GetResponse(action(httpClient));
                }
            }
        }

        private static string GetResponse(Task<HttpResponseMessage> task)
        {
            string response = String.Empty;
            task.ContinueWith((requestTask) =>
                {
                    HttpResponseMessage httpResponseMessage = requestTask.Result;
                    if (IsEnsureSuccessStatusCode)
                    {
                        httpResponseMessage.EnsureSuccessStatusCode();
                    }
                    return httpResponseMessage.Content.ReadAsStringAsync().Result;
                }
            )
            .ContinueWith((readTask) =>
                {
                    response = readTask.Result;
                }
            )
            .Wait();
            return response;
        }
    }
}