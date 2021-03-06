using System;
using System.Collections.Generic;
using System.Diagnostics;
using FewBox.Core.Utility.Formatter;
using FewBox.Core.Utility.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FewBox.Core.Utility.UnitTest
{
    [TestClass]
    public class NetUnitTest
    {
        private string BaseUrl { get; set; }

        [TestInitialize]
        public void Init()
        {
            this.BaseUrl = "https://httpbin.org";
        }

        [TestMethod]
        public void TestHttpPureGet()
        {
            HttpUtility.IsCertificateNeedValidate = false;
            HttpUtility.IsEnsureSuccessStatusCode = false;
            string url = $"{this.BaseUrl}/base64/RmV3Qm94";
            string token = "<token>";
            string response = HttpUtility.Get(url, token, new List<Header> { });
            Assert.AreEqual("FewBox", response);
        }

        [TestMethod]
        public void TestRestfulUtilityGet()
        {
            string url = $"{this.BaseUrl}/get";
            string token = "<token>";
            var response = RestfulUtility.Get<Response>(url, token, new List<Header> { });
            Assert.IsNotNull(response);
            Assert.AreEqual($"Bearer {token}", response.Headers["Authorization"].Value);
        }

        [TestMethod]
        public void TestRestfulUtilityGetDash()
        {
            string url = $"{this.BaseUrl}/anything";
            var anything = RestfulUtility.Get<Anything>(url, new List<Header> { });
            Assert.IsNotNull(anything);
            Assert.IsTrue(anything.Headers.User_Agent.Contains("FewBox"));
        }

        [TestMethod]
        public void TestRestfulUtilityPost()
        {
            string url = $"{this.BaseUrl}/post";
            string token = "<token>";
            var response = RestfulUtility.Post<Request, Response>(url, token, new Package<Request>
            {
                Headers = new List<Header>{
                    new Header { Key = "Trace-Id", Value = "FEWBOX001" }
                },
                Body = new Request { Name = "FewBox" }
            });
            Assert.IsNotNull(response);
            Assert.AreEqual($"Bearer {token}", response.Headers["Authorization"].Value);
            Assert.AreEqual($"FEWBOX001", response.Headers["Trace-Id"].Value);
            Assert.AreEqual($"FewBox", response.Json["Name"].Value);
            Assert.AreEqual($"FewBox", JsonUtility.Deserialize<Request>(response.Data).Name);
        }

        [TestMethod]
        public void TestRestfulUtilityPut()
        {
            string url = $"{this.BaseUrl}/put";
            string token = "<token>";
            var response = RestfulUtility.Put<Request, Response>(url, token, new Package<Request>
            {
                Headers = new List<Header>{
                    new Header { Key = "Trace-Id", Value = "FEWBOX001" }
                },
                Body = new Request { Name = "FewBox" }
            });
            Assert.IsNotNull(response);
            Assert.AreEqual($"Bearer {token}", response.Headers["Authorization"].Value);
            Assert.AreEqual($"FEWBOX001", response.Headers["Trace-Id"].Value);
            Assert.AreEqual($"FewBox", response.Json["Name"].Value);
            Assert.AreEqual($"FewBox", JsonUtility.Deserialize<Request>(response.Data).Name);
        }

        [TestMethod]
        public void TestRestfulUtilityPatch()
        {
            string url = $"{this.BaseUrl}/patch";
            var response = RestfulUtility.Patch<Request, Response>(url, new Package<Request>
            {
                Headers = new List<Header>{
                    new Header { Key = "Trace-Id", Value = "FEWBOX001" }
                },
                Body = new Request { Name = "FewBox" }
            }, "application/json-patch+json"); // application/json-patch+json application/merge-patch+json
            Assert.IsNotNull(response);
            Assert.AreEqual($"FEWBOX001", response.Headers["Trace-Id"].Value);
            Assert.AreEqual($"FewBox", response.Json["Name"].Value);
            Assert.AreEqual($"FewBox", JsonUtility.Deserialize<Request>(response.Data).Name);
        }

        [TestMethod]
        public void TestRestfulUtilityDelete()
        {
            string url = $"{this.BaseUrl}/delete";
            string token = "<token>";
            var response = RestfulUtility.Delete<Response>(url, token, new List<Header>{
                new Header { Key = "Trace-Id", Value = "FEWBOX001" }
            });
            Assert.IsNotNull(response);
            Assert.AreEqual($"Bearer {token}", response.Headers["Authorization"].Value);
            Assert.AreEqual($"FEWBOX001", response.Headers["Trace-Id"].Value);
        }

        //[TestMethod]
        public void TestPerformance()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int index = 0; index < 200; index++)
            {
                string url = $"https://raw.githubusercontent.com/FewBox/fewbox.github.io/master/version.json";
                var response = RestfulUtility.Get<Object>(url, new List<Header> { });
                string responseString = response.ToString();
                Assert.IsNotNull(responseString);
            }
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 5000);
        }

        //[TestMethod]
        public void TestWebSocketUtility()
        {
            string result = WebSocketUtility.Post("wss://mesh.fewbox.com:6443/api/v1/namespaces/fewbox-staging/pods/auth-deployment-latest-c56c67c8-vj7ph/exec?command=/bin/bash&stdin=true&stderr=true&stdout=true&tty=true&container=auth",
            "",
            new List<Header> { }).Result;
            //Assert.Fail(result);
        }

        //[TestMethod]
        public void TestAuthUtility()
        {
            var result = RestfulUtility.Get<PayloadResponseDto<IList<string>>>($"http://116.196.120.216/api/auth/Auth/Nodes/Get", new List<Header> {
                new Header { Key="Connection", Value="keep-alive" },
                new Header { Key="Content-Type", Value="application/json" },
                new Header { Key="Accept", Value="application/json" },
                new Header { Key="Accept-Encoding", Value="gzip, deflate, br" },
                new Header { Key="Accept-Language", Value="en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7" },
                new Header { Key="Authorization", Value="null" },
                new Header { Key="Host", Value="localhost:5001" },
                new Header { Key="Referer", Value="http://localhost/master/node" },
                new Header { Key="User-Agent", Value="Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36" },
                new Header { Key="Origin", Value="http://localhost" },
                new Header { Key="Sec-Fetch-Mode", Value="cors" },
                new Header { Key="Sec-Fetch-Site", Value="same-site" }
            });
            //Assert.Fail(result);
        }

        private class Request
        {
            public string Name { get; set; }
        }

        private class Response
        {
            public dynamic Args { get; set; }
            public dynamic Headers { get; set; }
            public dynamic Data { get; set; }
            public dynamic Json { get; set; }
            public string Url { get; set; }
        }

        private class Anything
        {
            public Headers Headers { get; set; }
        }

        private class Headers
        {
            [JsonProperty("User-Agent")]
            public string User_Agent { get; set; }
        }

        private class PayloadResponseDto<T>
        {
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
            public string ErrorCode { get; set; }
            public T Payload { get; set; }
        }
    }
}
