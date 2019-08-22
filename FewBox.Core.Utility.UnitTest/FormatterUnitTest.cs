using System;
using FewBox.Core.Utility.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FewBox.Core.Utility.UnitTest
{
    [TestClass]
    public class FormatterUnitTest
    {
        [TestMethod]
        public void TestBase64Utility()
        {
            string helloWorld = "Hello World!";
            string helloWorldBase64String = Base64Utility.Serialize(helloWorld);
            Assert.AreEqual("Hello World!", Base64Utility.Deserialize(helloWorldBase64String));
        }

        [TestMethod]
        public void TestJsonUtility()
        {
            var helloWorld = new { Greeting = "Hello World!" };
            string helloWorldJsonString = JsonUtility.Serialize(helloWorld);
            Assert.AreEqual("Hello World!", JsonUtility.Deserialize<dynamic>(helloWorldJsonString).Greeting.ToString());
        }

        [TestMethod]
        public void TestJsonUtilityDash()
        {
            var dash = new Dash { Dash_Test = "Dash-Test" };
            string dashJsonString = JsonUtility.Serialize(dash);
            Assert.AreEqual("Dash-Test", JsonUtility.Deserialize<Dash>(dashJsonString).Dash_Test.ToString());
        }

        [TestMethod]
        public void TestJsonUtilityWithCamelCase()
        {
            JsonUtility.IsCamelCase = true;
            var helloWorld = new { Greeting = "Hello World!" };
            string helloWorldJsonString = JsonUtility.Serialize(helloWorld);
            Assert.AreEqual("Hello World!", JsonUtility.Deserialize<dynamic>(helloWorldJsonString).greeting.ToString());
            JsonUtility.IsCamelCase = false;
        }

        [TestMethod]
        public void TestJsonUtilityIgnoreNull()
        {
            var helloWorld = new HelloWorld { Greeting = "Hello World!" };
            JsonUtility.IsNullIgnore = true;
            string helloWorldJsonString = JsonUtility.Serialize(helloWorld);
            Assert.IsFalse(helloWorldJsonString.Contains("Null"));
            JsonUtility.IsNullIgnore = false;
            helloWorldJsonString = JsonUtility.Serialize(helloWorld);
            Assert.IsTrue(helloWorldJsonString.Contains("Null"));
        }

        [TestMethod]
        public void TestXmlUtility()
        {
            var helloWorld = new HelloWorld { Greeting = "Hello World!" };
            string helloWorldXmlString = XmlUtility.Serialize(helloWorld);
            Assert.AreEqual("Hello World!", XmlUtility.Deserialize<HelloWorld>(helloWorldXmlString).Greeting);
        }

        [TestMethod]
        public void TestBinaryUtility()
        {
            var helloWorld = new HelloWorld { Greeting = "Hello World!" };
            var helloWorldBinary = BinaryUtility.Serialize(helloWorld);
            Assert.AreEqual("Hello World!", BinaryUtility.Deserialize<HelloWorld>(helloWorldBinary).Greeting);
        }

        [TestMethod]
        public void TestYamlUtility()
        {
            var helloWorld = new HelloWorld { Greeting = "Hello World!" };
            var helloWorldYaml = YamlUtility.Serialize(helloWorld);
            Assert.AreEqual("Hello World!", YamlUtility.Deserialize<HelloWorld>(helloWorldYaml).Greeting);
        }

        [Serializable]
        public class HelloWorld
        {
            public string Greeting { get; set; }
            public string Null { get; set; }
        }

        [Serializable]
        public class Dash
        {
            [JsonProperty("dash-test")]
            public string Dash_Test { get; set; }
        }
    }
}
