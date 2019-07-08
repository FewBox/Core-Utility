using System;
using System.Collections.Generic;
using System.Diagnostics;
using FewBox.Core.Utility.Formatter;
using FewBox.Core.Utility.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void TestRestfulUtilityGet()
        {
            string url = $"{this.BaseUrl}/get";
            string token = "<token>";
            var response = RestfulUtility.Get<Response>(url, token, new List<Header> { });
            Assert.IsNotNull(response);
            Assert.AreEqual($"Bearer {token}", response.Headers["Authorization"].Value);
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
            });
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
            for(int index=0;index<200;index++)
            {
                string url = $"https://raw.githubusercontent.com/FewBox/fewbox.github.io/master/version.json";
                var response = RestfulUtility.Get<Object>(url, new List<Header>{});
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
            new List<Header>{}).Result;
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
    }
}
