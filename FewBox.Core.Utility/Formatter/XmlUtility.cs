using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FewBox.Core.Utility.Formatter
{
    public static class XmlUtility
    {
        public static string Serialize<T>(T obj)
        {
            string serializedString = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                Serialize<T>(obj, stringWriter);
                stringWriter.Flush();
                serializedString = stringWriter.ToString();
            }
            return serializedString;
        }

        public static string Serialize<T>(T obj, Type[] extraTypes)
        {
            string serializedString = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                Serialize(obj, stringWriter, extraTypes);
                stringWriter.Flush();
                serializedString = stringWriter.ToString();
            }
            return serializedString;
        }

        public static void Serialize<T>(T obj, StringWriter writer)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(writer, obj);
        }

        public static void Serialize<T>(T obj, StringWriter writer, Type[] extraTypes)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), extraTypes);
            xmlSerializer.Serialize(writer, obj);
        }

        public static void Serialize<T>(T obj, XmlWriter writer)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(writer, obj);
        }

        public static void Serialize<T>(T obj, XmlWriter writer, Type[] extraTypes)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), extraTypes);
            xmlSerializer.Serialize(writer, obj);
        }

        public static T Deserialize<T>(StringReader reader)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(reader);
        }

        public static T Deserialize<T>(StringReader reader, Type[] extraTypes)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T), extraTypes);
            return (T)xs.Deserialize(reader);
        }

        public static T Deserialize<T>(XmlReader reader)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(reader);
        }

        public static T Deserialize<T>(XmlReader reader, Type[] extraTypes)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T), extraTypes);
            return (T)xs.Deserialize(reader);
        }

        public static T Deserialize<T>(string serializedString)
        {
            T t = default(T);
            if (String.IsNullOrEmpty(serializedString))
            {
                return t;
            }
            using (StringReader stringReader = new StringReader(serializedString))
            {
                t = Deserialize<T>(stringReader);
            }
            return t;
        }

        public static T Deserialize<T>(string serializedString, Type[] extraTypes)
        {
            T t = default(T);
            if (String.IsNullOrEmpty(serializedString))
            {
                return t;
            }
            using (StringReader stringReader = new StringReader(serializedString))
            {
                t = Deserialize<T>(stringReader, extraTypes);
            }
            return t;
        }

        public static void SaveAs<T>(T Obj, string fileName,
                           Encoding encoding, Type[] extraTypes)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(fileName));
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            XmlDocument document = new XmlDocument();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.Encoding = encoding;
            xmlWriterSettings.CloseOutput = true;
            xmlWriterSettings.CheckCharacters = false;
            using (Stream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    if (extraTypes != null)
                    {
                        Serialize<T>(Obj, writer, extraTypes);
                    }
                    else
                    {
                        Serialize<T>(Obj, writer);
                    }
                    writer.Flush();
                    document.Save(writer);
                }
            }
        }

        public static void SaveAs<T>(T Obj, string filename, Type[] extraTypes)
        {
            SaveAs<T>(Obj, filename, Encoding.UTF8, extraTypes);
        }

        public static void SaveAs<T>(T Obj, string filename, Encoding encoding)
        {
            SaveAs<T>(Obj, filename, encoding, null);
        }

        public static void SaveAs<T>(T Obj, string filename)
        {
            SaveAs<T>(Obj, filename, Encoding.UTF8);
        }

        public static T Open<T>(string filename, Type[] extraTypes)
        {
            T obj = default(T);
            if (File.Exists(filename))
            {
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                xmlReaderSettings.CloseInput = true;
                xmlReaderSettings.CheckCharacters = false;
                using (XmlReader reader = XmlReader.Create(filename, xmlReaderSettings))
                {
                    reader.ReadOuterXml();
                    if (extraTypes != null)
                        obj = Deserialize<T>(reader, extraTypes);
                    else
                        obj = Deserialize<T>(reader);
                }
            }
            return obj;
        }

        public static T Open<T>(string FileName)
        {
            return Open<T>(FileName, null);
        }
    }
}
