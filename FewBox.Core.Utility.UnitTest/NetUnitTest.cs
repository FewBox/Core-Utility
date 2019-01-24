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
            this.BaseUrl = "https://jsonplaceholder.typicode.com";
        }

        [TestMethod]
        public void TestRestfulUtilityGet()
        {
            string url = $"{this.BaseUrl}/posts/1";
            var post = RestfulUtility.Get<Post>(url, new List<Header>{});
            Assert.IsNotNull(post);
            Assert.AreEqual(1, post.Id);
        }

        [TestMethod]
        public void TestRestfulUtilityPost()
        {
            string url = $"{this.BaseUrl}/posts";
            var post = RestfulUtility.Post<Post, Post>(url, new Package<Post>{ 
                Headers = new List<Header>{
                    new Header { Key = "Token", Value = "FEW-BOX" }
                },
                Body = new Post { UserId = 1, Title = "Hello", Body = "World!" }
            });
            Assert.IsNotNull(post);
            Assert.AreEqual(101, post.Id);
        }

        [TestMethod]
        public void TestRestfulUtilityPut()
        {
            string url = $"{this.BaseUrl}/posts/1";
            var post = RestfulUtility.Put<Post, Post>(url, new Package<Post>{ 
                Headers = new List<Header>{
                    new Header { Key = "Token", Value = "FEW-BOX" }
                },
                Body = new Post { UserId = 1, Title = "Hello", Body = "World!" }
            });
            Assert.IsNotNull(post);
            Assert.AreEqual(1, post.UserId);
        }

        [TestMethod]
        public void TestRestfulUtilityPatch()
        {
            string url = $"{this.BaseUrl}/posts/1";
            var post = RestfulUtility.Put<Post, Post>(url, new Package<Post>{ 
                Headers = new List<Header>{
                    new Header { Key = "Token", Value = "FEW-BOX" }
                },
                Body = new Post { Title = "FewBox" }
            });
            Assert.IsNotNull(post);
            Assert.AreEqual("FewBox", post.Title);
        }

        [TestMethod]
        public void TestRestfulUtilityDelete()
        {
            string url = $"{this.BaseUrl}/posts/1";
            var post = RestfulUtility.Delete<Object>(url, new List<Header>{});
            Assert.IsNotNull(post);
        }

        private class Post
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
    }
}
