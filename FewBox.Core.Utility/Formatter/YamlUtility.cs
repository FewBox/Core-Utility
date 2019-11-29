using System;
using System.IO;
using YamlDotNet.Serialization;

namespace FewBox.Core.Utility.Formatter
{
    public static class YamlUtility
    {
        public static string Serialize<T>(T obj)
        {
            string yamlString = String.Empty;
            var yamlSerializer = new SerializerBuilder().Build();
            using (StringWriter stringWriter = new StringWriter())
            {
                yamlSerializer.Serialize(stringWriter, obj);
                yamlString = stringWriter.ToString();
            }
            return yamlString;
        }

        public static T Deserialize<T>(string yamlString) where T : class
        {
            T yamlObject = default(T);
            var yamlDeserializer = new DeserializerBuilder().Build();
            using (var stringReader = new StringReader(yamlString))
            {
                yamlObject = yamlDeserializer.Deserialize(stringReader, typeof(T)) as T;
            }
            return yamlObject;
        }
    }
}
