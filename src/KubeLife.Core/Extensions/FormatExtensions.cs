using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace KubeLife.Core.Extensions
{
    public static class FormatExtensions
    {
        private const string CnstDateTimeFilter = "dd.MM.yyyy HH:mm:ss";
        public static string ToDateFormat(this DateTime? source)
        {
            return source == null ? "[Not Found]" : ((DateTime)source).ToString(CnstDateTimeFilter);
        }

        public static string ToDateFormat(this DateTime source)
        {
            return source == null ? "[Not Found]" : (source).ToString(CnstDateTimeFilter);
        }

        public static string TextToHtml(this string text) => text.Replace("\n", "<br/>");

        public static string ToStringRaw(this Stream source)
        {
            StreamReader reader = new StreamReader(source);
            return reader.ReadToEnd();
        }

        public static byte[] ToByteArray(this Stream source)
        {
            using (var ms = new MemoryStream())
            {
                source.CopyTo(ms);
                return ms.ToArray();
            }

            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //    return ms.ToArray();
            //}
        }
    }
}
