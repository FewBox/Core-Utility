using FewBox.Core.Utility.Formatter;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FewBox.Core.Utility.Net
{
    public static class RestfulUtility
    {
        public static bool IsCertificateNeedValidate { private get; set; }
        public static bool IsLogging { private get; set; }
        public static bool IsEnsureSuccessStatusCode { private get; set; }
        public static TimeSpan Timeout { private get; set; }

        static RestfulUtility()
        {
            IsLogging = false;
            IsCertificateNeedValidate = true;
            IsEnsureSuccessStatusCode = true;
            Timeout = TimeSpan.FromMinutes(1);
        }

        #region Normal

        public static O Post<B, O>(string url, Package<B> package) where O : class
        {
            string responseString = String.Empty;
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.PostAsync(url, ConvertBodyObjectToStringContent(package.Body));
            }, package.Headers);
        }

        public static O Post<B, O>(string url, string token, Package<B> package) where O : class
        {
            string responseString = String.Empty;
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.PostAsync(url, ConvertBodyObjectToStringContent(package.Body));
            }, token, package.Headers);
        }

        public static O Put<B, O>(string url, Package<B> package) where O : class
        {
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.PutAsync(url, ConvertBodyObjectToStringContent(package.Body));
            }, package.Headers);
        }

        public static O Put<B, O>(string url, string token, Package<B> package) where O : class
        {
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.PutAsync(url, ConvertBodyObjectToStringContent(package.Body));
            }, token, package.Headers);
        }

        public static O Patch<B, O>(string url, Package<B> package, string mediaType) where O : class
        {
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.PatchAsync(url, ConvertBodyObjectToStringContent(package.Body, mediaType));
            }, package.Headers);
        }

        public static O Patch<B, O>(string url, string token, Package<B> package, string mediaType) where O : class
        {
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.PatchAsync(url, ConvertBodyObjectToStringContent(package.Body, mediaType));
            }, token, package.Headers);
        }

        public static O Delete<O>(string url, IList<Header> headers) where O : class
        {
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.DeleteAsync(url);
            }, headers);
        }

        public static O Delete<O>(string url, string token, IList<Header> headers) where O : class
        {
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.DeleteAsync(url);
            }, token, headers);
        }

        public static O Get<O>(string url, IList<Header> headers) where O : class
        {
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.GetAsync(url);
            }, headers);
        }

        public static O Get<O>(string url, string token, IList<Header> headers) where O : class
        {
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.GetAsync(url);
            }, token, headers);
        }

        # endregion

        # region Special

        public static O Head<O>(string url, IList<Header> headers) where O : class
        {
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.HeadAsync(url);
            }, headers);
        }

        public static O Head<O>(string url, string token, IList<Header> headers) where O : class
        {
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.HeadAsync(url);
            }, token, headers);
        }

        public static O Connect<O>(string url, IList<Header> headers) where O : class
        {
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.ConnectAsync(url);
            }, headers);
        }

        public static O Connect<O>(string url, string token, IList<Header> headers) where O : class
        {
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.ConnectAsync(url);
            }, token, headers);
        }

        public static O Options<O>(string url, IList<Header> headers) where O : class
        {
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.OptionsAsync(url);
            }, headers);
        }

        public static O Options<O>(string url, string token, IList<Header> headers) where O : class
        {
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.OptionsAsync(url);
            }, token, headers);
        }

        public static O Trace<O>(string url, IList<Header> headers) where O : class
        {
            return WapperHttpClient<O>((httpClient) =>
            {
                return httpClient.TraceAsync(url);
            }, headers);
        }

        public static O Trace<O>(string url, string token, IList<Header> headers) where O : class
        {
            return WapperHttpClientWithToken<O>((httpClient) =>
            {
                return httpClient.TraceAsync(url);
            }, token, headers);
        }

        # endregion

        private static StringContent ConvertBodyObjectToStringContent<T>(T body)
        {
            return ConvertBodyObjectToStringContent(body, "application/json");
        }

        private static StringContent ConvertBodyObjectToStringContent<T>(T body, string mediaType)
        {
            string jsonString = JsonUtility.Serialize<T>(body);
            if (IsLogging)
            {
                Console.WriteLine(jsonString);
            }
            return new StringContent(jsonString, Encoding.UTF8, mediaType);
        }

        private static void InitHeadersObjectToHttpRequestHeaders(HttpClient httpClient, IList<Header> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.Clear();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("FewBox/0.1 (Linux x86_64)");
        }

        private static O WapperHttpClient<O>(Func<HttpClient, Task<HttpResponseMessage>> action, IList<Header> headers) where O : class
        {
            return WapperHttpClientWithToken<O>(action, String.Empty, headers);
        }

        private static O WapperHttpClientWithToken<O>(Func<HttpClient, Task<HttpResponseMessage>> action, string token, IList<Header> headers) where O : class
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
                    return GetResponse<O>(action(httpClient));
                }
            }
        }

        private static O GetResponse<O>(Task<HttpResponseMessage> task) where O : class
        {
            O response = default(O);
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
                    response = JsonUtility.Deserialize<O>(readTask.Result);
                }
            )
            .Wait();
            return response;
        }
    }
}