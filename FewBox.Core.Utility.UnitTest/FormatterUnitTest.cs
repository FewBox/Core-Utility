using System;
using System.Collections.Generic;
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
            string helloWorldYaml = YamlUtility.Serialize(helloWorld);
            Assert.AreEqual("Hello World!", YamlUtility.Deserialize<HelloWorld>(helloWorldYaml).Greeting);
            var complexObject = new Pod
            {
                ApiVersion = "v1",
                Kind = "Pod",
                Metadata = new Metadata{
                    Annotations = new Dictionary<string, string> {{"sidecar.istio.io/status", "{\"version\":\"887285bb7fa76191bf7f637f283183f0ba057323b078d44c3db45978346cbc1a\",\"initContainers\":[\"istio-init\"],\"containers\":[\"istio-proxy\"],\"volumes\":[\"istio-envoy\",\"istio-certs\"],\"imagePullSecrets\":null}"}}
                    CreationTimestamp = DateTime.Now,
                    Labels = new Dictionary<string, string> {{"app","fewbox"},{"pod-template-hash","76d889886b"},{"version","v1"}},
                    Name = "fewbox-76d889886b-9dtk",
                    Namespace = "fewbox",
                    ResourceVersion = "32616085",
                    SelfLink = "/api/v1/namespaces/fewbox/pods/fewbox-76d889886b-9dtk6",
                    Uid = "49bb928f-d384-11e9-b929-fa163eab28fb"
                },
                Spec = new PodSpec
                {
                    Containers = new List<Container>
                    {
                        new Container 
                        {
                            Image = "111.111.111.111:5000/fewbox/fewbox:v1",
                            ImagePullPolicy = "IfNotPresent",
                            Name = "fewbox",
                            Ports = new List<Container_Port>
                            {
                                new Container_Port
                                {
                                    ContainerPort = 80
                                }
                            }
                        }
                    }
                }
            };
            string complexObjectYaml = YamlUtility.Serialize(complexObject);
        }

        [Serializable]
        public class HelloWorld
        {
            public string Greeting { get; set; }
            public string Null { get; set; }
        }

        public class Pod : Resource<PodSpec, PodStatus>
        {
        }

        public class PodSpec : Spec
        {
            public IList<object> Volumes { get; set; }
            public IList<Container> Containers { get; set; }
            public string NodeName { get; set; }
            public string ServiceAccountName { get; set; }
        }

        public class PodStatus : Status
        {
            public string HostIP { get; set; }
            public string PodIP { get; set; }
        }

        public class Container
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public string ImagePullPolicy { get; set; }
            public IList<Container_Port> Ports { get; set; }
            public string[] Command { get; set; }
            public IList<VolumeMount> VolumeMounts { get; set; }
        }

        public class VolumeMount
        {
            public string Name { get; set; }
            public string MountPath { get; set; }
            public string SubPath { get; set; }
            public bool ReadOnly { get; set; }
        }

        public class Container_Port
        {
            public int ContainerPort { get; set; }
        }

        public abstract class Resource<S, T> where S : Spec where T : Status
        {
            public string ApiVersion { get; set; }
            public string Kind { get; set; }
            public Metadata Metadata { get; set; }
            public S Spec { get; set; }
            public T Status { get; set; }
        }

        public class Metadata
        {
            public string Namespace { get; set; }
            public string Name { get; set; }
            public string SelfLink { get; set; }
            public string Uid { get; set; }
            public string ResourceVersion { get; set; }
            public int? Generation { get; set; }
            public DateTime? CreationTimestamp { get; set; }
            public IDictionary<string, string> Labels { get; set; }
            public IDictionary<string, string> Annotations { get; set; }
        }

        public class Spec
        {
        }

        public class Status
        {
            public string Phase { get; set; }
        }

        [Serializable]
        public class Dash
        {
            [JsonProperty("dash-test")]
            public string Dash_Test { get; set; }
        }
    }
}
