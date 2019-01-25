using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace FewBox.Core.Utility.Formatter
{
    public static class BinaryUtility
    {
        public static byte[] Serialize(object value)
        {
            byte[] valueBytes;
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, value);
                valueBytes = memoryStream.ToArray();
            }
            return valueBytes;
        }

        public static T Deserialize<T>(byte[] valueBytes) where T : class
        {
            T value;
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                memoryStream.Write(valueBytes, 0, valueBytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                value = binaryFormatter.Deserialize(memoryStream) as T;
            }
            return value;
        }
    }
}