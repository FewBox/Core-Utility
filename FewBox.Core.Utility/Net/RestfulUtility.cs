using FewBox.Core.Utility.Formatter;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;

namespace FewBox.Core.Utility.Net
{
    public static class RestfulUtility
    {
        public static O Post<B, O>(string url, Package<B> package) where O : class
        {
            string responseString = String.Empty;
            WapperHttpClient((httpClient) => {
                HttpResponseMessage response = httpClient.PostAsync(url,
                ConvertBodyObjectToStringContent(package.Body)).Result;
                response.EnsureSuccessStatusCode();
                responseString = response.Content.ReadAsStringAsync().Result;
            }, package.Headers);
            return JsonUtility.Deserialize<O>(responseString);
        }

        public static O Put<B, O>(string url, Package<B> package) where O : class
        {
            string responseString = String.Empty;
            WapperHttpClient((httpClient) =>
            {
                httpClient.PutAsync(url,
                ConvertBodyObjectToStringContent(package.Body)).ContinueWith(
                    (requestTask) =>
                    {
                        HttpResponseMessage response = requestTask.Result;
                        response.EnsureSuccessStatusCode();
                        response.Content.ReadAsStringAsync().ContinueWith(
                            (readTask) =>
                            {
                                responseString = readTask.Result;
                            });
                    }
                ).Wait();
            }, package.Headers);
            return JsonUtility.Deserialize<O>(responseString);
        }

        public static O Patch<B, O>(string url, Package<B> package) where O : class
        {
            string responseString = String.Empty;
            WapperHttpClient((httpClient) =>
            {
                httpClient.PatchAsync(url,
                ConvertBodyObjectToStringContent(package.Body)).ContinueWith(
                    (requestTask) =>
                    {
                        HttpResponseMessage response = requestTask.Result;
                        response.EnsureSuccessStatusCode();
                        response.Content.ReadAsStringAsync().ContinueWith(
                            (readTask) =>
                            {
                                responseString = readTask.Result;
                            });
                    }
                ).Wait();
            }, package.Headers);
            return JsonUtility.Deserialize<O>(responseString);
        }
        
        public static O Delete<O>(string url, IList<Header> headers) where O : class
        {
            string responseString = String.Empty;
            WapperHttpClient((httpClient) =>
            {
                httpClient.DeleteAsync(url).ContinueWith(
                    (requestTask) =>
                    {
                        HttpResponseMessage response = requestTask.Result;
                        response.EnsureSuccessStatusCode();
                        response.Content.ReadAsStringAsync().ContinueWith(
                            (readTask) =>
                            {
                                responseString = readTask.Result;
                            });
                    }
                ).Wait();
            }, headers);
            return JsonUtility.Deserialize<O>(responseString);
        }

        public static O Get<O>(string url, IList<Header> headers) where O : class
        {
            string responseString = String.Empty;
            WapperHttpClient((httpClient) =>
            {
                httpClient.GetAsync(url).ContinueWith(
                    (requestTask) =>
                    {
                        HttpResponseMessage response = requestTask.Result;
                        response.EnsureSuccessStatusCode();
                        response.Content.ReadAsStringAsync().ContinueWith(
                            (readTask) =>
                            {
                                responseString = readTask.Result;
                            });
                    }
                ).Wait();
            }, headers);
            return JsonUtility.Deserialize<O>(responseString);
        }

        private static StringContent ConvertBodyObjectToStringContent<T>(T body)
        {
            string jsonString = JsonUtility.Serialize<T>(body);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }

        private static void InitHeadersObjectToHttpRequestHeaders(HttpClient httpClient, IList<Header> headers)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "FewBox Validation");
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        private static void WapperHttpClient(Action<HttpClient> action, IList<Header> headers)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                InitHeadersObjectToHttpRequestHeaders(httpClient, headers);
                action(httpClient);
            }
        }
    }
}