using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace KubeLife.Core.Extensions
{
    public static class CoreExtensions
    {
        public static bool IsAny<T>(this IEnumerable<T> source) => source != null && source.Any();

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> functoApply)
        {
            foreach (var itm in source)
                functoApply(itm);
        }

        public static List<Tout> CasttoList<Tout, Kin>(this Kin[] source, Func<Kin, Tout> methodConvert)
        {
            if (!source.IsAny())
                return new List<Tout>();

            var target = new List<Tout>();
            foreach (var itm in source)
                target.Add(methodConvert(itm));

            return target;
        }

        public static List<Tout> CasttoList<Tout, Kin>(this List<Kin> source, Func<Kin, Tout> methodConvert)
        {
            if (!source.IsAny())
                return new List<Tout>();

            var target = new List<Tout>();
            foreach (var itm in source)
                target.Add(methodConvert(itm));

            return target;
        }

        public static T ChangeType<T>(this object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            return (T)tc.ConvertFrom(value);
        }

        public static string ToStringForm(this Stream source)
        {
            using(var sr = new StreamReader(source))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
