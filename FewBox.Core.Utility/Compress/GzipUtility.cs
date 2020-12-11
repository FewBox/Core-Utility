using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace FewBox.Core.Utility.Compress
{
    public static class GzipUtility
    {
        public static string Zip(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var outputStream = new MemoryStream())
            {
                using (var compressedStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    using (var inputStream = new MemoryStream(bytes))
                    {
                        inputStream.CopyTo(compressedStream);
                    }
                }
                var outputBytes = outputStream.ToArray();
                return Convert.ToBase64String(outputBytes);
            }
        }

        public static string Unzip(string input)
        {
            var inputBytes = Convert.FromBase64String(input);
            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream(inputBytes))
                {
                    using (var decompressedStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        decompressedStream.CopyTo(outputStream);
                    }
                }
                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }
}