using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace FewBox.Core.Utility.Formatter
{
    public static class JsonUtility
    {
        public static bool IsCamelCase { private get; set; }
        public static string Serialize<T>(T obj)
        {
            string jsonString = String.Empty;
            var jsonSerializer = GetJsonSerializer();
            using (var stringWriter = new StringWriter())
            {
                jsonSerializer.Serialize(stringWriter, obj);
                jsonString = stringWriter.ToString();
            }
            return jsonString;
        }

        public static T Deserialize<T>(string jsonString) where T : class
        {
            T jsonObject = default(T);
            var jsonSerializer = GetJsonSerializer();
            using (var stringReader = new StringReader(jsonString))
            {
                jsonObject = jsonSerializer.Deserialize(stringReader, typeof(T)) as T;
            }
            return jsonObject;
        }

        private static JsonSerializer GetJsonSerializer()
        {
            JsonSerializer jsonSerializer;
            if(IsCamelCase)
            {
                var jsonSerializerSettings = new JsonSerializerSettings{
                    ContractResolver = new DefaultContractResolver {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                };
                jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
            }
            else
            {
                jsonSerializer = new JsonSerializer();
            }
            return jsonSerializer;
        }
    }
}
