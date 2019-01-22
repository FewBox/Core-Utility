using Newtonsoft.Json;
using System;
using System.IO;

namespace FewBox.Core.Utility.Formatter
{
    public static class JsonUtility
    {
        public static string Serialize<T>(T obj)
        {
            string jsonString = String.Empty;
            JsonSerializer jsonSerializer = new JsonSerializer();
            using (StringWriter stringWriter = new StringWriter())
            {
                jsonSerializer.Serialize(stringWriter, obj);
                jsonString = stringWriter.ToString();
            }
            return jsonString;
        }

        public static T Deserialize<T>(string jsonString) where T : class
        {
            T jsonObject = default(T);
            JsonSerializer jsonSerializer = new JsonSerializer();
            using (TextReader stringReader = new StringReader(jsonString))
            {
                jsonObject = jsonSerializer.Deserialize(stringReader, typeof(T)) as T;
            }
            return jsonObject;
        }
    }
}
