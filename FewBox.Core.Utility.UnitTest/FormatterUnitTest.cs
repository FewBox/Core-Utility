using System;
using FewBox.Core.Utility.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void TestJsonUtilityWithCamelCase()
        {
            JsonUtility.IsCamelCase = true;
            var helloWorld = new { Greeting = "Hello World!" };
            string helloWorldJsonString = JsonUtility.Serialize(helloWorld);
            Assert.AreEqual("Hello World!", JsonUtility.Deserialize<dynamic>(helloWorldJsonString).greeting.ToString());
            JsonUtility.IsCamelCase = false;
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
        }
    }
}
