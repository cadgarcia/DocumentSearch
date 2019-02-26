using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EngenhariaFiles
{
    public static class Extensions
    {
        public static String BytesToString(long byteCount)
        {
            var suf = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
            {
                return "0" + suf[0];
            }
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }

        public  static void XMLSave(string fileName, Configuracao config)
        {
            using (var writer = new System.IO.StreamWriter(fileName))
            {
                var serializer = new XmlSerializer(config.GetType());
                serializer.Serialize(writer, config);
                writer.Flush();
            }
        }

        public static Configuracao XMLLoad(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var stream = System.IO.File.OpenRead(fileName))
                {
                    var serializer = new XmlSerializer(typeof(Configuracao));
                    return serializer.Deserialize(stream) as Configuracao;
                }
            }
            return new Configuracao();
        }

        public static string Base64Encode(this string plainText)
        {
            if (String.IsNullOrEmpty(plainText))
            {
                return "";
            }

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            if (String.IsNullOrEmpty(base64EncodedData))
            {
                return "";
            }

            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
